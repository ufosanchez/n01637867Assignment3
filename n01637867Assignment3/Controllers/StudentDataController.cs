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
    public class StudentDataController : ApiController
    {
        // The database context class which allows to access to the MySQL Database
        private SchoolDbContext School = new SchoolDbContext();

        //This Controller access the Student table of the School DB
        /// <summary>
        /// A GET method which will return a list of all Students in the system
        /// </summary>
        /// <example>GET api/StudentData/ListStudents</example>
        /// <returns>
        /// A list of students objects
        /// GET api/StudentData/ListStudents => 
        /// [{"EnrolDate": "2018-06-18", "StudentFName": "Sarah", "StudentID": 1, "StudentLName": "Valdez", "StudentNumber": "N1678"},
        ///  {"EnrolDate": "2018-08-02", "StudentFName": "Jennifer", "StudentID": 2, "StudentLName": "Faulkner", "StudentNumber": "N1679"}, ...]
        /// </returns>
        [HttpGet]
        [Route("api/StudentData/ListStudents")]
        public List<Student> ListStudents()
        {
            //connect to the school database
            MySqlConnection Conn = School.AccessDatabase();

            //open a connection to the database
            Conn.Open();

            //set up a string query "select * from students"
            string query = "select * from students";

            //create a sql command
            MySqlCommand cmd = Conn.CreateCommand();

            //execute the sql command
            cmd.CommandText = query;

            //store the result from the query in a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //an empty list of the students
            List<Student> Students = new List<Student> { };

            //loop through the result set
            while (ResultSet.Read())
            {
                //store in variables the values from the DB collected
                int StudentID = Convert.ToInt32(ResultSet["studentid"]);

                string StudentFName = ResultSet["studentfname"].ToString();

                string StudentLName = ResultSet["studentlname"].ToString();

                string StudentNumber = ResultSet["studentnumber"].ToString();

                DateTime EnrolDate = ResultSet.GetDateTime("enroldate");

                //create an instance of Student
                Student NewStudent = new Student();

                //assign to the properties the values
                NewStudent.StudentID = StudentID;
                NewStudent.StudentFName = StudentFName;
                NewStudent.StudentLName = StudentLName;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.EnrolDate = EnrolDate.ToString("yyyy-MM-dd");

                //add it to the Students list
                Students.Add(NewStudent);
            }

            //close the connection 
            Conn.Close();

            //return the list the students
            return Students;
        }

        /// <summary>
        /// This GET method returns an individual student from the database by specifying the primary key studentid
        /// </summary>
        /// <example>GET api/StudentData/FindStudent/22</example>
        /// <param name="StudentId">the student ID which the user is looking for</param>
        /// <returns>
        /// A student object which is the object that represent a student of the School DB
        /// GET api/StudentData/FindStudent/22 => {"EnrolDate": "2018-08-27", "StudentFName": "David", "StudentID": 22, "StudentLName": "Dunlap", "StudentNumber": "N1734"}
        /// GET api/StudentData/FindStudent/4 => {"EnrolDate": "2018-07-03", "StudentFName": "Mario", "StudentID": 4, "StudentLName": "English", "StudentNumber": "N1686"}
        /// </returns>
        [HttpGet]
        [Route("api/StudentData/FindStudent/{StudentId}")]
        public Student FindStudent(int StudentId)
        {
            //connect to the school database
            MySqlConnection Conn = School.AccessDatabase();

            //open a connection to the database
            Conn.Open();

            //create a sql command
            MySqlCommand cmd = Conn.CreateCommand();

            //execute the sql command
            cmd.CommandText = "select * from students where studentid = @key";

            //sanitizing the student find
            cmd.Parameters.AddWithValue("@key", StudentId.ToString());

            // create an instance of Student
            Student selectedStudent = new Student();

            //store the result from the query in a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //loop through the result set (it should be 1)
            while (ResultSet.Read())
            {
                //asign the values to the properties of the object
                selectedStudent.StudentID = Convert.ToInt32(ResultSet["studentid"]);
                selectedStudent.StudentFName = ResultSet["studentfname"].ToString();
                selectedStudent.StudentLName = ResultSet["studentlname"].ToString();
                selectedStudent.StudentNumber = ResultSet["studentnumber"].ToString();
                selectedStudent.EnrolDate = ResultSet.GetDateTime("enroldate").ToString("yyyy-MM-dd");
            }

            //close the connection 
            Conn.Close();

            //return the selected student 
            return selectedStudent;
        }
    }
}
