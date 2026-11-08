using System.ComponentModel.DataAnnotations;

namespace raceDay_Part3.Models
{//start of namespace
    public class EventViewModel
    {//start of class
        public int EventID { get; set; }

        [Required]
        public string EventName { get; set; } = "";

        public string? EventDescription { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string Location { get; set; } = "";

        public decimal EntryFee { get; set; }

        public int UserID { get; set; }

        public int CategoryID { get; set; }
    }//end of class
}//end of namespace