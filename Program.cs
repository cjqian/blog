using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using blog.Models;
using blog.Data;
using Microsoft.EntityFrameworkCore;

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

    public class ListCommentViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ListCommentViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int entryID)
        {
            //var comment = new Comment();
            //comment.EntryID = entryID;
            //return View("/Views/Shared/CommentList.cshtml");
            var comments = _context.Comment.Where(c => c.EntryID == entryID);
            return View("/Views/Shared/CommentList.cshtml", await comments.ToListAsync());
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
