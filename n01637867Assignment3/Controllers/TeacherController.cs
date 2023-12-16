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

        // GET : Teacher/Edit/{id}
        // Go to -> /View/Teacher/Edit.cshtml
        // Browser opens the page that contains the information of specific Teacher on the inputs
        /// <summary>
        /// Routes to a dynamically generated Teacher Edit Page that gather the information from the database.
        /// </summary>
        /// <param name="id">Id of the teacher taht want to be updated</param>
        /// <returns>A dynamic Update Teacher webpage which provides the current information of the Teacher and asks the user for new information as part of a form.</returns>
        /// <example>/Teacher/Edit/18</example>
        public ActionResult Edit(int id)
        {
            try
            {
                //use the teacher data controller
                TeacherDataController Controller = new TeacherDataController();

                //grab the teacher information from the DB
                Teacher SelectedTeacher = Controller.FindTeacher(id);

                return View(SelectedTeacher);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        //POST :  /Teacher/Update/{id}
        //receives the post data
        //call the database action to update the Teacher resource and redirect to the Teacher show page
        [HttpPost]
        public ActionResult Update(int id, Teacher UpdateTeacher)
        {
            //use of try and catch, if for any reason there is a typo or there is no connection to the database, the user will be redirected to an error page
            try
            {
                Debug.WriteLine("id that is gonna be updated" + id);

                Debug.WriteLine(UpdateTeacher.TeacherFName);
                Debug.WriteLine(UpdateTeacher.TeacherLName);
                Debug.WriteLine(UpdateTeacher.EmployeeNumber);
                Debug.WriteLine(UpdateTeacher.Salary);

                // use the teacher data controller
                TeacherDataController Controller = new TeacherDataController();

                //update the article in the system
                Controller.UpdateTeacher(id, UpdateTeacher);

                return RedirectToAction("Show/" + id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
            
        }

        // GET : /Teacher/Ajax_Update/{id}
        // Go to -> /View/Teacher/Ajax_Update.cshtml
        // Update the Teacher by using AJAX, this will no need a form submission 
        // Browser opens the page that contains the information of specific Teacher on the inputs
        /// <summary>
        /// Routes to a dynamically generated Teacher Edit Page that gather the information from the database.
        /// </summary>
        /// <param name="id">Id of the teacher that want to be updated</param>
        /// <returns>A dynamic Update Teacher webpage which provides the current information of the Teacher and asks the user for 
        /// new information using AJAX</returns>
        /// <example>/Teacher/Edit/18</example>
        public ActionResult Ajax_Update(int id)
        {
            //use the teacher data controller
            TeacherDataController Controller = new TeacherDataController();

            //grab the teacher information from the DB
            Teacher SelectedTeacher = Controller.FindTeacher(id);

            return View(SelectedTeacher);
        }
    }
}