using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace raceDay_Part3.Models
{//start of namespace 
    public class ProfileImageViewModel
    {//start of class
        public IFormFile? ProfileImage { get; set; }

        public string? ImagePath { get; set; }
    }//end of class
}//end of namespace