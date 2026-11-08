
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using raceDay_Part3.Models;
using raceDay_Part3.Services;

namespace raceDay_Part3.Controllers
{//start of namespace
    public class AccountController : Controller
    {//start of class
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string result = await _apiService.RegisterAsync(model);

            if (result.Contains("created", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Success"] = "Your account was created. Please log in.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", result);
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _apiService.LoginAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            if (!string.IsNullOrEmpty(result.Cookie))
            {
                HttpContext.Session.SetString(
                    "ApiSessionCookie",
                    result.Cookie
                );
            }

            using var document = JsonDocument.Parse(result.UserData!);

            var user = document.RootElement;

            HttpContext.Session.SetInt32(
                "UserID",
                user.GetProperty("userID").GetInt32()
            );

            HttpContext.Session.SetString(
                "FullName",
                user.GetProperty("fullName").GetString() ?? ""
            );

            HttpContext.Session.SetString(
                "Email",
                user.GetProperty("email").GetString() ?? ""
            );

            string role = user.GetProperty("role").GetString() ?? "";

            HttpContext.Session.SetString("Role", role);

            if (role == "Organizer")
            {
                return RedirectToAction("OrganizerDashboard", "Home");
            }
            else if (role == "Participant")
            {
                return RedirectToAction("ParticipantDashboard", "Home");
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {//start of method
            string? cookie = HttpContext.Session.GetString(
                "ApiSessionCookie"
            );

            await _apiService.LogoutAsync(cookie);

            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }//end of method
    }//end of  class
}//end of namespace
