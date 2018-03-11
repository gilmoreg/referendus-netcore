namespace referendus_netcore.Controllers
{
	using System.Diagnostics;
	using Microsoft.AspNetCore.Mvc;

	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}