using System;

namespace raceDay_Part3.Models
{//start of namespace 
    public class RecordResultViewModel
    {//start of class 
        public int EnrolmentID { get; set; }

        public int EventID { get; set; }

        public int Position { get; set; }

        public TimeSpan FinishTime { get; set; }
    }//end of class
}//end of namespace