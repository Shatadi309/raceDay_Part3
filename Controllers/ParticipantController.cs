
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using raceDay_Part3.Attributes;
using raceDay_Part3.Services;

namespace raceDay_Part3.Controllers
{//start of namespace
    [SessionAuthorize("Participant")]
    public class ParticipantController : Controller
    {//start of class
        private readonly ApiService _apiService;

        public ParticipantController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Events()
        {
            var events = await _apiService.GetEventsAsync();

            return View(events);
        }

        public async Task<IActionResult> EventDetails(int id)
        {
            var eventDetails = await _apiService.GetEventByIdAsync(id);

            if (eventDetails == null)
            {
                return NotFound();
            }

            return View(eventDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enrol(
            int eventID, int categoryID, decimal paymentAmount)
        {
            string? cookie = HttpContext.Session.GetString("ApiSessionCookie");

            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _apiService.EnrolInEventAsync(
                eventID,
                categoryID,
                paymentAmount,
                cookie
            );

            if (result.Success)
            {
                TempData["Success"] = "You have successfully enrolled in this event.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction("EventDetails", new { id = eventID });
        }//end of method

        // Display a participant's enrolments
        public async Task<IActionResult> MyEnrolments()
        {
            string? cookie = HttpContext.Session.GetString("ApiSessionCookie");

            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Account");
            }

            var enrolments = await _apiService.GetMyEnrolmentsAsync(cookie);

            return View(enrolments);
        }//end of displaying method

        // Display a participant's results
        public async Task<IActionResult> MyResults()
        {//start of displaying results method
            string? cookie = HttpContext.Session.GetString("ApiSessionCookie");

            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Account");
            }

            var results = await _apiService.GetMyResultsAsync(cookie);

            return View(results);
        }//end of displaying results method

        // Cancel an enrolment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelEnrolment(int id)
        {//start of cancel method
            string? cookie = HttpContext.Session.GetString("ApiSessionCookie");

            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Account");
            }

            string result = await _apiService.CancelEnrolmentAsync(id, cookie);

            if (result.Contains("successfully"))
            {
                TempData["Success"] = result;
            }
            else
            {
                TempData["Error"] = result;
            }

            return RedirectToAction("MyEnrolments");
        }//end of cancel method

    }//end of class
}//end of namespace
