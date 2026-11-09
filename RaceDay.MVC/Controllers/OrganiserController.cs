using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrganiserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/events", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Go back to dashboard
            }

            ViewBag.Error = "Failed to create event. Make sure all fields are correct.";
            return View(model);
        }
        public async Task<IActionResult> MyEvents()
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
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

            return View(events);
        }
        [HttpGet]
        public async Task<IActionResult> EventDetails(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            // 1. Get the event details (optional, but good for the header)
            var eventResponse = await client.GetAsync($"api/events/{id}");
            var eventItem = new EventViewModel();
            if (eventResponse.IsSuccessStatusCode)
            {
                var json = await eventResponse.Content.ReadAsStringAsync();
                eventItem = JsonSerializer.Deserialize<EventViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new EventViewModel();
            }

            // 2. Get the enrolments for this event
            var enrolResponse = await client.GetAsync($"api/enrolments/event/{id}");
            var enrolments = new List<dynamic>(); // Using dynamic for quick parsing

            if (enrolResponse.IsSuccessStatusCode)
            {
                var json = await enrolResponse.Content.ReadAsStringAsync();
                enrolments = JsonSerializer.Deserialize<List<dynamic>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<dynamic>();
            }

            ViewBag.EventName = eventItem.Name;
            ViewBag.EventId = id;

            return View(enrolments);
        }
    }
}