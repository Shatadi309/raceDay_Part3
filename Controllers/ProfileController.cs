using Microsoft.AspNetCore.Mvc;
using raceDay_Part3.Models;
using System.IO;

namespace raceDay_Part3.Controllers
{//start of namespace
    public class ProfileController : Controller
    {//start of class
        private readonly IWebHostEnvironment _environment;

        public ProfileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        //HttpGet to retrive the user's profile image
        [HttpGet]
        public IActionResult Index()
        {
            var model = new ProfileImageViewModel
            {
                ImagePath = HttpContext.Session.GetString("ProfileImage")
            };

            return View(model);
        }

        //HttpPost to store the user's 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(ProfileImageViewModel model)
        {//start method
            if (model.ProfileImage == null ||
                model.ProfileImage.Length == 0)
            {
                ModelState.AddModelError(
                    "ProfileImage",
                    "Please select an image."
                );

                model.ImagePath =
                    HttpContext.Session.GetString("ProfileImage");

                return View("Index", model);
            }

            if (model.ProfileImage.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError(
                    "ProfileImage",
                    "The image must be smaller than 5 MB."
                );

                model.ImagePath =
                    HttpContext.Session.GetString("ProfileImage");

                return View("Index", model);
            }

            var allowedExtensions = new[]
            {
                ".jpg", ".jpeg", ".png"
            };

            string extension = Path.GetExtension(
                model.ProfileImage.FileName
            ).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError(
                    "ProfileImage",
                    "Only JPG, JPEG and PNG images are allowed."
                );

                model.ImagePath =
                    HttpContext.Session.GetString("ProfileImage");

                return View("Index", model);
            }

            string uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "profiles"
            );

            Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid() + extension;

            string filePath = Path.Combine(
                uploadsFolder,
                fileName
            );

            using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await model.ProfileImage.CopyToAsync(stream);
            }

            model.ImagePath = "/uploads/profiles/" + fileName;

            HttpContext.Session.SetString(
                "ProfileImage",
                model.ImagePath
            );

            return RedirectToAction("Index");
        }//end of method
    }//end of class
}//end of namespace