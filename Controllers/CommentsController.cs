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
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Comment.ToListAsync());
        }

        // GET: Comments
        public async Task<IActionResult> List(int? entryID)
        {
            return View(await _context.Comment.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.ID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Content,EntryID")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Author = User.Identity.Name;
                
                if (comment.Author == null)
                {
                    comment.Author = "Anonymous";
                }
                comment.PublishDate = DateTime.Now;

                _context.Add(comment);

                await _context.SaveChangesAsync();

                //return RedirectToAction("Index");
                //return RedirectToAction("Explore", "Entries");
                return RedirectToAction("Details", "Entries", new { ID = comment.EntryID });
            }


             return RedirectToAction("Explore", "Entries");
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.ID == id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Author,Content,EntryID,PublishDate")] Comment comment)
        {
            if (id != comment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.ID))
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
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
 

            if (id == null)
            {
                return NotFound();
            }


            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.ID == id);
            if (comment == null)
            {
                return NotFound();
            }
            // We're not going to have delete confirmation for comments. 
            // Un-comment for confirmation.
            //return View(comment);

            return DeleteConfirmed(id); 
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.ID == id);
            var entryID = comment.EntryID;

            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Entries", new { id = entryID });
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.ID == id);
        }
    }
}
