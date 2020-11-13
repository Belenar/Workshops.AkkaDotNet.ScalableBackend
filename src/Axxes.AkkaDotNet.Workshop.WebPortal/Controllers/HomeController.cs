using System.Diagnostics;
using Axxes.AkkaDotNet.Workshop.WebPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
