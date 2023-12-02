using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace n01637867Assignment3.Models
{
    public class Teacher
    {

        //definition of my Teacher
        public int TeacherID { get; set; }
        public string TeacherFName { get; set; }
        public string TeacherLName { get; set; }
        public string EmployeeNumber { get; set; }
        public string HireDate { get; set; }
        public decimal Salary { get; set; }

        public bool IsValid()
        {
            bool valid = true;

            if (TeacherFName == null || TeacherLName == null || EmployeeNumber == null || HireDate == null || Salary == null )
            {
                //Base validation to check if the fields are entered.
                valid = false;
            }
            else
            {
                //Validation for fields 
                if (TeacherFName == "" || TeacherFName.Length < 2 || TeacherFName.Length > 255) valid = false;
                if (TeacherLName == "" || TeacherLName.Length < 2 || TeacherLName.Length > 255) valid = false;
                if (EmployeeNumber == "" || EmployeeNumber.Length < 2 || EmployeeNumber.Length > 255) valid = false;
                if (HireDate == "" || HireDate.Length < 2 || HireDate.Length > 255) valid = false;
                if (Salary  <= 0) valid = false;

            }
            Debug.WriteLine("The model validity is : " + valid);

            return valid;
        }

    }
}