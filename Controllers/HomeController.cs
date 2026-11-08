using Microsoft.AspNetCore.Mvc;
using raceDay_Part3.Models;
using System.Diagnostics;
using raceDay_Part3.Attributes;


namespace raceDay_Part3.Controllers
{//start of namespace
    public class HomeController : Controller
    {//start of class
        private readonly ILogger<HomeController> _logger;

        [SessionAuthorize("Participant", "Organizer")]
        //access denied method
        public IActionResult AccessDenied()
        {
            return View();
        }//end of method
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }//end of method


        //creating organizer dashboard
        [SessionAuthorize("Organizer")]
        public IActionResult OrganizerDashboard()
        {
            return View();
        }

        //creating the paricipate dashboard
        [SessionAuthorize("Participant")]
        public IActionResult ParticipantDashboard()
        {
            return View();
        }
    }//end of class
}//end of namespace
