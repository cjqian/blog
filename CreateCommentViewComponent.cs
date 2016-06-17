using blog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog
{
    public class CreateCommentViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int entryID)
        {
            var comment = new Comment();
            comment.EntryID = entryID;
            return View("/Views/Shared/CommentCreate.cshtml", comment);
        }
    }

}
