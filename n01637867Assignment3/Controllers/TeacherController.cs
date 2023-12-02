using n01637867Assignment3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // GET : Teacher/DeleteConfirm/{id}
        // Go to -> /View/Teacher/DeleteConfirm.cshtml
        // Browser confirm if the user wants to delete the selected teacher
        public ActionResult DeleteConfirm(int id)
        {
            //use the teacher data controller
            TeacherDataController Controller = new TeacherDataController();

            //grab the teacher information from the DB
            Teacher SelectedTeacher = Controller.FindTeacher(id);

            return View(SelectedTeacher);
        }

        // POST : Teacher/Delete
        // Go to -> /View/Teacher/DeleteConfirm.cshtml
        // Browser confirm if the user wants to delete the selected teacher
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //use the teacher data controller
            TeacherDataController Controller = new TeacherDataController();

            //grab the teacher information from the DB
            Controller.DeleteTeacher(id);

            return RedirectToAction("List");
        }

        //GET :  /Teacher/New
        // Go to -> /View/Teacher/New.cshtml
        // Display a form to insert a new Teacher
        public ActionResult New()
        {
            
            return View();
        }

        //POST :  /Teacher/Create
        //receives the post data
        //tries to send it to the API and redirects to the Teachers List page
        [HttpPost]
        public ActionResult Create(string TeacherFName, string TeacherLName, string EmployeeNumber, DateTime HireDate, decimal Salary)
        {
            Debug.WriteLine("Access!!");

            Debug.WriteLine(TeacherFName);
            Debug.WriteLine(TeacherLName);
            Debug.WriteLine(EmployeeNumber);
            Debug.WriteLine(HireDate);
            Debug.WriteLine(HireDate.ToString("yyyy-MM-dd"));
            Debug.WriteLine(Salary);

            Teacher NewTeacher = new Teacher();

            NewTeacher.TeacherFName = TeacherFName;
            NewTeacher.TeacherLName = TeacherLName;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate.ToString("yyyy-MM-dd");
            NewTeacher.Salary = Salary;

            //use the teacher data controller
            TeacherDataController Controller = new TeacherDataController();

            Controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }
    }
}