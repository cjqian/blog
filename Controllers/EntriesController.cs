using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Controllers
{
    [Authorize]
    public class EntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IAuthorizationService _authorizationService;

        public EntriesController(ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        // GET: Entries
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Explore()
        {
            return View(await _context.Entry.ToListAsync());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Profile(String ProfileID)
        {
            var publicEntries = await _context.Entry.Where(m => m.Author == ProfileID && m.IsPublic).ToListAsync();

            Profile profile = new Profile();
            profile.Author = ProfileID;
            profile.PublicEntries = publicEntries;

            // Only pass private entries if the user has the correct credentials.
            if (User.Identity.Name == ProfileID)
            {
                var privateEntries = await _context.Entry.Where(m => m.Author == ProfileID && !m.IsPublic).ToListAsync();
                profile.PrivateEntries = privateEntries;
            }

            return View(profile);
        }

        // GET: Entries/Details/5
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var entry = await _context.Entry.SingleOrDefaultAsync(m => m.ID == id);
            if (entry == null)
            {
                return View("Error");
            }

            // A user can't get details on a private post he/she did not author.
            if (!await _authorizationService.AuthorizeAsync(User, entry, new ViewRequirement()))
            {
                return View("Error");
            }

            return View(entry);
        }

        // GET: Entries/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Entries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,Title,IsPublic")] Entry entry)
        {
             if (ModelState.IsValid)
            {
                entry.Author = User.Identity.Name;
                entry.PublishDate = DateTime.Now;

                _context.Add(entry);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Entries", new { ID = entry.ID });
            }
            
            return View(entry);
        }

        // GET: Entries/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var entry = await _context.Entry.SingleOrDefaultAsync(m => m.ID == id);
            if (entry == null)
            {
                return View("Error");
            }

            //Only the author should be able to edit. 
            if (!await _authorizationService.AuthorizeAsync(User, entry, new ModifyRequirement()))
            {
                return View("Error");
            }

            return View(entry);
        }

        // POST: Entries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Content,Title,IsPublic")] Entry entry)
        {
            if (id != entry.ID)
            {
                return View("Error");
            }
           
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryExists(entry.ID))
                    {
                        return View("Error");
                    }
                    else
                    {
                        throw;
                    }
                }

                return View(entry);
            }

            return View(entry);
        }

        // GET: Entries/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var entry = await _context.Entry.SingleOrDefaultAsync(m => m.ID == id);
            if (entry == null)
            {
                return View("Error");
            }

            //Only the author should be able to delete. 
            if (!await _authorizationService.AuthorizeAsync(User, entry, new ModifyRequirement()))
            {

                return View("Error");
            }

            return View(entry);
        }

        // POST: Entries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entry = await _context.Entry.SingleOrDefaultAsync(m => m.ID == id);
            var author = entry.Author;

            _context.Entry.Remove(entry);
            await _context.SaveChangesAsync();
            return RedirectToAction("Profile", "Entries", new { ProfileID = author });
        }

        private bool EntryExists(int id)
        {
            return _context.Entry.Any(e => e.ID == id);
        }
    }
}
