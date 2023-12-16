using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace n01637867Assignment3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        //GET : /Home/Error
        // Go to -> /Home/Error.cshtml
        /// <summary>
        /// This window is for showing Generic Errors
        /// </summary>
        public ActionResult Error()
        {
            return View();
        }
    }
}
