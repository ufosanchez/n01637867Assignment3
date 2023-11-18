using n01637867Assignment3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace n01637867Assignment3.Controllers
{
    public class StudentController : Controller
    {

        //  GET: Student/List
        //  Go to  -> /Views/Student/List.cshtml
        //  Browser opens a list of Students page
        public ActionResult List()
        {
            //create an empty list of Students
            List<Student> Students = new List<Student>();

            //use the student data controller
            StudentDataController Controller = new StudentDataController();

            //grab the students information from the DB
            Students = Controller.ListStudents();

            //return the information to the view
            return View(Students);
        }

        // GET : Student/Show/{id}
        // Go to -> /View/Student/Show.cshtml
        // Browser opens the page of the selected id student

        public ActionResult Show(int id)
        {
            //use the student data controller
            StudentDataController Controller = new StudentDataController();

            //grab the student information from the DB
            Student SelectedStudent = Controller.FindStudent(id);

            //return the information to the view
            return View(SelectedStudent);
        }
    }
}