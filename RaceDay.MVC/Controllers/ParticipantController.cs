using Microsoft.AspNetCore.Mvc;

namespace RaceDay.MVC.Controllers
{
    public class ParticipantController : Controller
    {
        public IActionResult Index()
        {
            // Security check: Ensure user is logged in AND is a Participant
            if (HttpContext.Session.GetString("UserRole") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}