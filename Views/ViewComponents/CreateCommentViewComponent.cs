using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
{
    public class CreateCommentViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int entryID)
        {
            var comment = new Comment();
            comment.EntryID = entryID;
            return View(comment);
        }
    }

}
