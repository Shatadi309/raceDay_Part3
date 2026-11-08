namespace raceDay_Part3.Models
{//start of namespace
    public class EnrolmentViewModel
    {//start of class
        public int EnrolmentID { get; set; }

        public int UserID { get; set; }

        public int EventCategoryID { get; set; }

        public DateTime EnrolmentDate { get; set; }

        public string PaymentStatus { get; set; } = "";

        public decimal PaymentAmount { get; set; }

        public int EventID { get; set; }

        public string EventName { get; set; } = "";

        public DateTime EventDate { get; set; }

        public string Location { get; set; } = "";

        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = "";
    }//end of class
}//end of namespace