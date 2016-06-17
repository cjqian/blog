using blog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog
{
    public class DetailEntryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Entry entry, bool showComments)
        {
            if (showComments)
            {
                return View("/View/Entries/Details.cshtml", new { ID = entry.ID });
            }
            else return View("/Views/Shared/EntryPreview.cshtml", entry);
        }
    }
}
