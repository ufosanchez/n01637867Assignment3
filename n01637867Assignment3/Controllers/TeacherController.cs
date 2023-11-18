using n01637867Assignment3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace n01637867Assignment3.Controllers
{
    public class TeacherController : Controller
    {

        // GET: Teacher/List?TeacherSeacrhKey={value}
        // Go to  -> /Views/Teacher/List.cshtml
        //Browser opens a list of Teachers page
        public ActionResult List(string TeacherSeacrhKey = null)
        {
            //create an empty list of Teacher
            List<Teacher> Teachers = new List<Teacher>();

            //use the teacher data controller
            TeacherDataController Controller = new TeacherDataController();

            //grab the teachers information from the DB
            Teachers = Controller.ListTeachers(TeacherSeacrhKey);

            //return the information to the view
            return View(Teachers);
        }

        // GET : Teacher/Show/{id}
        // Go to -> /View/Teacher/Show.cshtml
        // Browser opens the page of the selected id teacher
        public ActionResult Show(int id)
        {
            //use the teacher data controller
            TeacherDataController Controller = new TeacherDataController();

            //grab the teacher information from the DB
            Teacher SelectedTeacher = Controller.FindTeacher(id);

            return View(SelectedTeacher);
        }
    }
}