using Blog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
{
    public class ListCommentViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ListCommentViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int entryID)
        {
            var comments = _context.Comment.Where(c => c.EntryID == entryID);
            return View(await comments.ToListAsync());
        }
    }

}

