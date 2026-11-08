using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models.Dtos;
using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/auth/login", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);
                var user = doc.RootElement.GetProperty("user");

                HttpContext.Session.SetString("UserRole", user.GetProperty("role").GetString()!);
                HttpContext.Session.SetString("UserName", user.GetProperty("name").GetString()!);
                HttpContext.Session.SetString("UserId", user.GetProperty("userId").GetInt32().ToString());

                if (HttpContext.Session.GetString("UserRole") == "Organiser")
                    return RedirectToAction("Index", "Organiser");
                else
                    return RedirectToAction("Index", "Participant");
            }

            ViewBag.Error = "Invalid email or password.";
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/auth/register", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            var error = await response.Content.ReadAsStringAsync();
            ViewBag.Error = "Registration failed. " + error;
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}