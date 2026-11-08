using System.ComponentModel.DataAnnotations;

namespace raceDay_Part3.Models
{//start of namespace
    public class LoginViewModel
    {//start of class
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }//end of class
}//end of namespace