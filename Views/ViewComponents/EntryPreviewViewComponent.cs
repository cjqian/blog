using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
{
    public class EntryPreviewViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Entry entry)
        {
            return View(entry);
        }
    }
}

