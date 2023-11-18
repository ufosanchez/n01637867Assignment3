using n01637867Assignment3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace n01637867Assignment3.Controllers
{
    public class ClassController : Controller
    {

        // GET: Class/List
        // Go to  -> /Views/Class/List.cshtml
        // Browser opens a list of classes page
        public ActionResult List()
        {
            //create an empty list of Classes
            List<Course> Classes = new List<Course>();

            //use the course data controller
            ClassDataController Controller = new ClassDataController();

            //grab the classes information from the DB
            Classes = Controller.ListClasses();

            //pass the classes information to the view
            return View(Classes);
        }

        // GET : Class/Show/{id}
        // Go to -> /View/Class/Show.cshtml
        // Browser opens the page of the selected id class
        public ActionResult Show(int id)
        {
            //use the course data controller
            ClassDataController Controller = new ClassDataController();

            //grab the class information from the DB
            Course SelectedClass = Controller.FindClass(id);

            //pass the class information to the view
            return View(SelectedClass);
        }
    }
}