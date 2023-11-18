using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace n01637867Assignment3.Models
{
    public class Course
    {
        //definition of my Course
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public int TeacherID { get; set; }
        public string StartDate { get; set; }
        public string FinishDate { get; set; }
        public string ClassName { get; set; }
    }
}