using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;

namespace Blog.Controllers
{
    public class EntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntriesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Entries
        public async Task<IActionResult> Explore()
        {
            return View(await _context.Entry.ToListAsync());
        }

        public async Task<IActionResult> Profile(String ProfileID)
        {
            var publicEntries = await _context.Entry.Where(m => m.Author == ProfileID && m.IsPublic).ToListAsync();

            Profile profile = new Profile();
            profile.Author = ProfileID;
            profile.PublicEntries = publicEntries;

            if (User.Identity.Name == ProfileID)
            {
                var privateEntries = await _context.Entry.Where(m => m.Author == ProfileID && !m.IsPublic).ToListAsync();
                profile.PrivateEntries = privateEntries;
            }

            return View(profile);
        }

        // GET: Entries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.Entry.SingleOrDefaultAsync(m => m.ID == id);
            if (entry == null)
            {
                return NotFound();
            }

            // A user can't get details on a private post he/she did not author.
            if (!entry.IsPublic && User.Identity.Name != entry.Author)
            {
                return new BadRequestObjectResult("You are not authorized to view this post.");
            }

            return View(entry);
        }

        // GET: Entries/Create
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
            //Only logged-in users can create a post.
            if (User.Identity.Name == null)
            {
                return new BadRequestObjectResult("Only logged-in users can create posts. How did you even get here?");
            }

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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.Entry.SingleOrDefaultAsync(m => m.ID == id);
            if (entry == null)
            {
                return NotFound();
            }

            //Only the author of the post has permission to edit.
            if (entry.Author != User.Identity.Name)
            {
                return new BadRequestObjectResult("You do not have permission to edit this post.");
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
                return NotFound();
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
                        return NotFound();
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.Entry.SingleOrDefaultAsync(m => m.ID == id);
            if (entry == null)
            {
                return NotFound();
            }
            

            //Only the author of the entry can delete.
            if (entry.Author != User.Identity.Name)
            {
                return new BadRequestObjectResult("You do not have permission to delete this post.");
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
