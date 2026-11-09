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

        // --- 1. DASHBOARD (SHOW EVENTS) ---
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var response = await client.GetAsync("api/events");
            var events = new List<EventViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                events = JsonSerializer.Deserialize<List<EventViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<EventViewModel>();
            }
            return View(events);
        }

        // --- 2. ENROL ---
        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId, int categoryId)
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var enrolData = new { EventId = eventId, CategoryId = categoryId };
            var jsonContent = new StringContent(JsonSerializer.Serialize(enrolData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/enrolments", jsonContent);

            if (response.IsSuccessStatusCode)
                TempData["SuccessMessage"] = "Successfully enrolled!";
            else
                TempData["ErrorMessage"] = "Failed to enrol.";

            return RedirectToAction("Index");
        }

        // --- 3. MY ENROLMENTS ---
        [HttpGet]
        public async Task<IActionResult> MyEnrolments()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var response = await client.GetAsync("api/enrolments/my-enrolments");
            var enrolments = new List<EnrolmentViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                // Let the deserializer do the heavy lifting
                enrolments = JsonSerializer.Deserialize<List<EnrolmentViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<EnrolmentViewModel>();
            }
            else
            {
                ViewBag.Error = "API Error: " + response.StatusCode;
            }

            return View(enrolments);
        }

        // --- 4. MY RESULTS ---
        [HttpGet]
        public async Task<IActionResult> MyResults()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var response = await client.GetAsync("api/results/my-results");
            var results = new List<ResultViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                results = JsonSerializer.Deserialize<List<ResultViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ResultViewModel>();
            }
            else
            {
                ViewBag.Error = "API Error: " + response.StatusCode;
            }

            return View(results);
        }
    }
}