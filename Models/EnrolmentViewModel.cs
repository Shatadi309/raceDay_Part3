
using System;

namespace raceDay_Part3.Models
{//start of namespace
    public class EnrolmentViewModel
    {//start of class
        public int EnrolmentID { get; set; }

        public DateTime EnrolmentDate { get; set; }

        public string PaymentStatus { get; set; } = string.Empty;

        public decimal PaymentAmount { get; set; }

        public int EventID { get; set; }

        public string EventName { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public string Location { get; set; } = string.Empty;

        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = string.Empty;
    }//end of class
}//end of class
