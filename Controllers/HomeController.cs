using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Models.ManageViewModels;
namespace blog.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

                return View();

        }

        public IActionResult Profile()
        {
            ViewData["Message"] = "Your application description page.";

            return RedirectToAction("Profile", "Entries");
        }

        public IActionResult Explore()
        {
            ViewData["Message"] = "Your Explore page.";

            return RedirectToAction("Explore", "Entries");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
