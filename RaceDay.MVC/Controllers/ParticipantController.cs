using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ParticipantController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Participant Dashboard
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var response = await client.GetAsync("api/events");

            var events = new List<EventViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                events = JsonSerializer.Deserialize<List<EventViewModel>>(jsonString, options) ?? new List<EventViewModel>();
            }
            else
            {
                ViewBag.Error = "Failed to load events. API Status: " + response.StatusCode;
            }

            return View(events);
        }

        // POST: Enrol in an event
        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId, int categoryId)
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var enrolData = new
            {
                EventId = eventId,
                CategoryId = categoryId
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(enrolData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/enrolments", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Successfully enrolled!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to enrol. You might already be enrolled.";
            }

            return RedirectToAction("Index");
        }

        // GET: My Enrolments
        [HttpGet]
        public async Task<IActionResult> MyEnrolments()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var response = await client.GetAsync("api/enrolments/my-enrolments");

            var enrolments = new List<EnrolmentViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                enrolments = JsonSerializer.Deserialize<List<EnrolmentViewModel>>(jsonString, options) ?? new List<EnrolmentViewModel>();
            }
            else
            {
                ViewBag.Error = "Failed to load enrolments. API Status: " + response.StatusCode;
            }

            return View(enrolments);
        }
    }
}