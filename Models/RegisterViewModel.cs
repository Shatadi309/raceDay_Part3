using System.ComponentModel.DataAnnotations;

namespace raceDay_Part3.Models
{//start of namespace
    public class RegisterViewModel
    {//start of class
        [Required]
        public string FullName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        public string? PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; } = "Participant";
    }//end of class
}//end of namespace