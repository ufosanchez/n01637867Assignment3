using MySql.Data.MySqlClient;
using n01637867Assignment3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace n01637867Assignment3.Controllers
{
    public class ClassDataController : ApiController
    {
        // The database context class which allows to access to the MySQL Database
        private SchoolDbContext School = new SchoolDbContext();

        //This Controller access the Class table of the School DB
        /// <summary>
        /// A GET method which will return a list of all Classes in the system
        /// </summary>
        /// <example>GET api/ClassData/ListClasses</example>
        /// <returns>
        /// A list of classes objects
        /// GET api/ClassData/ListClasses => 
        /// [{"ClassCode": "http5101", "ClassID": 1, "ClassName": "Web Application Development", "FinishDate": "2018-12-14", "StartDate": "2018-09-04", "TeacherID": 1},
        ///  {"ClassCode": "http5102", "ClassID": 2, "ClassName": "Project Management", "FinishDate": "2018-12-14", "StartDate": "2018-09-04", "TeacherID": 2}, ...]
        /// </returns>
        [HttpGet]
        [Route("api/ClassData/ListClasses")]
        public List<Course> ListClasses()
        {
            //connect to the school database
            MySqlConnection Conn = School.AccessDatabase();

            //open a connection to the database
            Conn.Open();

            //set up a string query "select * from classes"
            string query = "select * from classes";

            //create a sql command
            MySqlCommand cmd = Conn.CreateCommand();

            //execute the sql command
            cmd.CommandText = query;

            //store the result from the query in a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //an empty list of the classes of data type Course
            List<Course> Classes = new List<Course> { };

            //loop through the result set
            while (ResultSet.Read())
            {
                //store in varbales the values from the DB collected
                int ClassID = Convert.ToInt32(ResultSet["classid"]);

                string ClassCode = ResultSet["classcode"].ToString();

                int TeacherID = Convert.ToInt32(ResultSet["teacherid"]);

                DateTime StartDate = ResultSet.GetDateTime("startdate");

                DateTime FinishDate = ResultSet.GetDateTime("finishdate");

                string ClassName = ResultSet["classname"].ToString();

                //create an instance of Course
                Course NewClass = new Course();

                //assign to the properties the values
                NewClass.ClassID = ClassID;
                NewClass.ClassCode = ClassCode;
                NewClass.TeacherID = TeacherID;
                NewClass.StartDate = StartDate.ToString("yyyy-MM-dd");
                NewClass.FinishDate = FinishDate.ToString("yyyy-MM-dd");
                NewClass.ClassName = ClassName;

                //add it to the Course list
                Classes.Add(NewClass);
            }

            //close the connection 
            Conn.Close();

            //return the list of the name of the teachers 
            return Classes;
        }

        /// <summary>
        /// This GET method returns an individual class from the database by specifying the primary key classid
        /// </summary>
        /// <example>GET api/ClassData/FindClass/8</example>
        /// <param name="ClassId">the class ID which the user is looking for</param>
        /// <returns>
        /// A course object which is the object that represent a class of the School DB
        /// GET api/ClassData/FindClass/8 => {"ClassCode": "http5203", "ClassID": 8, "ClassName": "XML and Web Services", "FinishDate": "2019-04-27", "StartDate": "2019-01-08", "TeacherID": 4}
        /// </returns>
        [HttpGet]
        [Route("api/ClassData/FindClass/{ClassId}")]
        public Course FindClass(int ClassId)
        {
            //connect to the school database
            MySqlConnection Conn = School.AccessDatabase();

            //open a connection to the database
            Conn.Open();

            //create a sql command
            MySqlCommand cmd = Conn.CreateCommand();

            //execute the sql command
            cmd.CommandText = "select * from classes where classid = @key";

            //sanitizing the student find
            cmd.Parameters.AddWithValue("@key", ClassId.ToString());

            // create an instance of Course
            Course selectedClass = new Course();

            //store the result from the query in a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //loop through the result set (it should be 1)
            while (ResultSet.Read())
            {
                //asign the values to the properties of the object
                selectedClass.ClassID = Convert.ToInt32(ResultSet["classid"]);
                selectedClass.ClassCode = ResultSet["classcode"].ToString();
                selectedClass.TeacherID = Convert.ToInt32(ResultSet["teacherid"]);
                selectedClass.StartDate = ResultSet.GetDateTime("startdate").ToString("yyyy-MM-dd");
                selectedClass.FinishDate = ResultSet.GetDateTime("finishdate").ToString("yyyy-MM-dd");
                selectedClass.ClassName = ResultSet["classname"].ToString();
            }

            //close the connection 
            Conn.Close();

            //return the selected class
            return selectedClass;

        }
    }
}
