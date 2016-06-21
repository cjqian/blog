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
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IAuthorizationService _authorizationService;

        public CommentsController(ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        // GET: Comments/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,EntryID")] Comment comment)
        {
            var entry = _context.Entry.SingleOrDefaultAsync(e => e.ID == comment.EntryID);

            // We need to make sure a comment is not made on a private post if the commenter is not the author of the post.
            if (!await _authorizationService.AuthorizeAsync(User, entry, new ViewRequirement()))
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                comment.Author = User.Identity.Name;

                comment.PublishDate = DateTime.Now;

                _context.Add(comment);

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Entries", new { ID = comment.EntryID });
            }

            //An error occured
            //Was it the comment length? We have to check here instead of returning the comment because the comments exist in a partial view.
            if (comment.Content.Length < comment.MIN_LENGTH || comment.Content.Length > comment.MAX_LENGTH)
            {
                return new BadRequestObjectResult("Your comment has to be at least (" + comment.MIN_LENGTH + ") characters and less than (" + comment.MAX_LENGTH + ") characters.");
            }

            return RedirectToAction("Details", "Entries", new { ID = comment.EntryID });
        }

        // GET: Comments/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.ID == id);
            if (comment == null)
            {
                return View("Error");
            }

            //Only the author should be able to delete. 
            if (!await _authorizationService.AuthorizeAsync(User, comment, new ModifyRequirement()))
            {
                return View("Error");
            }

            return View(comment);
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
