using System;
using System.ComponentModel.DataAnnotations;

namespace raceDay_Part3.Models
{//start of namespace
    public class EventViewModel
    {//start class
        public int EventID { get; set; }

        [Required(ErrorMessage = "Please enter the event name.")]
        public string EventName { get; set; } = "";

        public string? EventDescription { get; set; }

        [Required(ErrorMessage = "Please select the event date.")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Please enter the event location.")]
        public string Location { get; set; } = "";

        [Range(0, 100000, ErrorMessage = "Entry fee cannot be negative.")]
        public decimal EntryFee { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage = "Please select an event category.")]
        public int CategoryID { get; set; }
    }//end of class
}//end of namespace