using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using blog.Data;
using blog.Models;

namespace blog.Controllers
{
    public class EntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntriesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Entries
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Entries
        public async Task<IActionResult> Explore()
        {
            return View(await _context.Entry.ToListAsync());
        }


        public async Task<IActionResult> Profile()
        {
            return View(await _context.Entry.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("ID,Author,Content,PublishDate,Title,IsPublic")] Entry entry)
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
            return View(entry);
        }

        // POST: Entries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Author,Content,PublishDate,Title,IsPublic")] Entry entry)
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
                return RedirectToAction("Index");
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

            return View(entry);
        }

        // POST: Entries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entry = await _context.Entry.SingleOrDefaultAsync(m => m.ID == id);
            _context.Entry.Remove(entry);
            await _context.SaveChangesAsync();
            return RedirectToAction("Profile", "Entries");
        }

        private bool EntryExists(int id)
        {
            return _context.Entry.Any(e => e.ID == id);
        }

    }
}
