
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using raceDay_Part3.Attributes;
using raceDay_Part3.Models;
using raceDay_Part3.Services;
using System;
using System.Threading.Tasks;

namespace raceDay_Part3.Controllers
{//start of namespace
    [SessionAuthorize("Organizer")]
    public class EventsController : Controller
    {//start of class
        private readonly ApiService _apiService;

        public EventsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Display all events
        public async Task<IActionResult> Index()
        {
            var events = await _apiService.GetEventsAsync();

            return View("EventIndex", events);
        }

        // Display the Create Event page
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _apiService.GetCategoriesAsync();

            return View();
        }

        // Create an event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            if (model.CategoryID <= 0)
            {
                ModelState.AddModelError(
                    "CategoryID",
                    "Please select an event category.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _apiService.GetCategoriesAsync();

                return View(model);
            }

            string? cookie = HttpContext.Session.GetString("ApiSessionCookie");

            string result = await _apiService.CreateEventAsync(model, cookie);

            if (string.IsNullOrWhiteSpace(result))
            {
                TempData["Success"] = "Event created successfully.";

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result);

            ViewBag.Categories = await _apiService.GetCategoriesAsync();

            return View(model);
        }

        // Display the Edit Event page
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var eventDetails = await _apiService.GetEventByIdAsync(id);

            if (eventDetails == null)
                return NotFound("Event does not exist.");

            return View(eventDetails);
        }

        // Update an event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventViewModel model)
        {
            if (id != model.EventID)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            string result = await _apiService.UpdateEventAsync(model);

            if (string.IsNullOrWhiteSpace(result) ||
                result.Contains("updated", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Success"] = "Event updated successfully.";

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result);

            return View(model);
        }

        // Display the Delete Event page
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var eventDetails = await _apiService.GetEventByIdAsync(id);

            if (eventDetails == null)
                return NotFound("Event does not exist.");

            return View(eventDetails);
        }

        // Delete an event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string result = await _apiService.DeleteEventAsync(id);

            if (string.IsNullOrWhiteSpace(result))
            {
                TempData["Success"] = "Event deleted successfully.";

                return RedirectToAction("Index");
            }

            TempData["Error"] = result;

            return RedirectToAction("Index");
        }

        // Display participants enrolled in an event
        public async Task<IActionResult> Enrolments(int id)
        {
            string? cookie = HttpContext.Session.GetString("ApiSessionCookie");

            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Account");
            }

            var enrolments = await _apiService.GetEventEnrolmentsAsync(id, cookie);

            ViewBag.EventID = id;

            return View(enrolments);
        }

        // Display results for a specific event
        public async Task<IActionResult> Results(int id)
        {
            string? cookie = HttpContext.Session.GetString("ApiSessionCookie");

            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Account");
            }

            var results = await _apiService.GetEventResultsAsync(id, cookie);

            ViewBag.EventID = id;

            return View(results);
        }

        // Display the form to record a result
        [HttpGet]
        public IActionResult RecordResult(int id, int eventID)
        {
            var model = new RecordResultViewModel
            {
                EnrolmentID = id,
                EventID = eventID
            };

            return View(model);
        }

        // Save a participant's result
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecordResult(RecordResultViewModel model)
        {
            string? cookie = HttpContext.Session.GetString("ApiSessionCookie");

            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Account");
            }

            if (model.Position <= 0)
            {
                ModelState.AddModelError(
                    "Position",
                    "Please enter a valid finishing position.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string result = await _apiService.RecordResultAsync(
                model.EnrolmentID,
                model.Position,
                model.FinishTime,
                cookie);

            if (result.Contains("recorded", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Success"] = "Race result recorded successfully.";

                return RedirectToAction(
                    "Enrolments",
                    new { id = model.EventID });
            }

            ModelState.AddModelError("", result);

            return View(model);
        }//end of method
    }//end of class
}//end of namespace 
