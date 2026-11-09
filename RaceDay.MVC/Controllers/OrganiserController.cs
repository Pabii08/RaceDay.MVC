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
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Failed to create event.";
            return View(model);
        }

        // --- Make sure this only appears ONCE ---
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
            else
            {
                ViewBag.Error = "Failed to load events.";
            }

            return View(events);
        }
        // ----------------------------------------

        [HttpGet]
        public async Task<IActionResult> EventDetails(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            // 1. Get Event Name
            var eventResponse = await client.GetAsync($"api/events/{id}");
            var eventItem = new EventViewModel();
            if (eventResponse.IsSuccessStatusCode)
            {
                var json = await eventResponse.Content.ReadAsStringAsync();
                eventItem = JsonSerializer.Deserialize<EventViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new EventViewModel();
            }

            // 2. Get Enrolments
            var enrolResponse = await client.GetAsync($"api/enrolments/event/{id}");
            var enrolments = new List<ParticipantEnrolmentViewModel>();

            if (enrolResponse.IsSuccessStatusCode)
            {
                var json = await enrolResponse.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                // Manually map the API JSON to our ViewModel
                foreach (var element in doc.RootElement.EnumerateArray())
                {
                    enrolments.Add(new ParticipantEnrolmentViewModel
                    {
                        EnrolmentId = element.GetProperty("enrolmentId").GetInt32(),
                        ParticipantName = element.GetProperty("user").GetProperty("name").GetString() ?? "",
                        ParticipantEmail = element.GetProperty("user").GetProperty("email").GetString() ?? "",
                        CategoryName = element.GetProperty("category").GetProperty("name").GetString() ?? "",
                        EnrolmentDate = element.GetProperty("enrolmentDate").GetDateTime()
                    });
                }
            }

            var viewModel = new EventDetailsViewModel
            {
                EventName = eventItem.Name,
                Enrolments = enrolments
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CaptureResult(int enrolmentId, string time, int position)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            // Convert string time (e.g. "01:45:30") to TimeSpan format for the API
            var timeSpan = TimeSpan.Parse(time);

            var resultData = new
            {
                EnrolmentId = enrolmentId,
                Time = timeSpan,
                Position = position,
                Status = "Completed"
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(resultData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/results", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Result captured successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to capture result.";
            }

            // Redirect back to the same page (you might want to pass the eventId back properly in a real app)
            return RedirectToAction("MyEvents");
        }
    }
}