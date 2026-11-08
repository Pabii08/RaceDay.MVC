using Microsoft.AspNetCore.Mvc;

namespace RaceDay.MVC.Controllers
{
    public class OrganiserController : Controller
    {
        public IActionResult Index()
        {
            // Security check: Ensure user is logged in AND is an Organiser
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}