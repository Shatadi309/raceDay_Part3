
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace raceDay_Part3.Models
{//start of namespace
    public class EventViewModel
    {//start of class
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

        public int CategoryID { get; set; }

        // Categories available for this event
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }//end of class
}//end of namespace
