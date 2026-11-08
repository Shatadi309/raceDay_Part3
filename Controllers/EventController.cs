using Microsoft.AspNetCore.Mvc;
using raceDay_Part3.Attributes;
using raceDay_Part3.Models;
using raceDay_Part3.Services;
using System;
using Microsoft.AspNetCore.Http;

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
        }//end of display method

        // Display the Create Event form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _apiService.GetCategoriesAsync();

            return View();
        }

        // Submit a new event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            // Check that a category has been selected
            if (model.CategoryID <= 0)
            {
                ModelState.AddModelError(
                    "CategoryID",
                    "Please select an event category.");
            }

            // Check whether the form is valid
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _apiService.GetCategoriesAsync();

                return View(model);
            }

            // Get the API session cookie
            string? cookie = HttpContext.Session.GetString("ApiSessionCookie");

            // Send the event to the API
            string result = await _apiService.CreateEventAsync(model, cookie);

            // Check whether the event was created successfully
            if (string.IsNullOrWhiteSpace(result))
            {
                TempData["Success"] = "Event created successfully.";

                return RedirectToAction("Index");
            }

            // Display an error if the event was not created
            ModelState.AddModelError("", result);

            ViewBag.Categories = await _apiService.GetCategoriesAsync();

            return View(model);
        }//end of method


        // Display the Edit Event form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var eventDetails = await _apiService.GetEventByIdAsync(id);

            if (eventDetails == null)
            {
                return NotFound("Event does not exist.");
            }

            return View(eventDetails);
        }

        // Submit the updated event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventViewModel model)
        {
            if (id != model.EventID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

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


        // Display the Delete Event confirmation page
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var eventDetails = await _apiService.GetEventByIdAsync(id);

            if (eventDetails == null)
            {
                return NotFound("Event does not exist.");
            }

            return View(eventDetails);
        }

        // Confirm and delete the event
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
        }//end of confirm and delete

        // View participants enrolled in an event
        public async Task<IActionResult> Enrolments(int id)
        {//start of method
            string? cookie = HttpContext.Session.GetString("ApiSessionCookie");

            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Account");
            }

            var enrolments = await _apiService.GetEventEnrolmentsAsync(id, cookie);

            ViewBag.EventID = id;

            return View(enrolments);
        }//end of method

    }//end of class
}//end of namespace