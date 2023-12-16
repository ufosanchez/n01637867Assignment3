using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Google.Protobuf;
using MySql.Data.MySqlClient;
using n01637867Assignment3.Models;

namespace n01637867Assignment3.Controllers
{
    public class TeacherDataController : ApiController
    {
        // The database context class which allows to access to the MySQL Database
        private SchoolDbContext School = new SchoolDbContext();

        //This Controller access the Teacher table of the School DB
        /// <summary>
        /// A GET method which will return a list of all Classes in the system if the user doesn't provide a data on the text box,
        /// if the user filter the query it will shpw the teacher found
        /// </summary>
        /// <example>
        /// GET api/TeacherData/ListTeachers
        /// GET api/TeacherData/ListTeachers/cody
        /// GET api/TeacherData/ListTeachers/morris
        /// </example>
        /// <returns>
        /// A list of teachers objects or the objectd of the teacher that teh user is looking fo
        /// GET api/TeacherData/ListTeachers => 
        /// [{"EmployeeNumber": "T378", "HireDate": "2016-08-05", "Salary": 55.30, "TeacherFName": "Alexander", "TeacherID": 1, "TeacherLName": "Bennett"},
        ///  {"EmployeeNumber": "T381", "HireDate": "2014-06-10", "Salary": 62.77, "TeacherFName": "Caitlin", "TeacherID": 2, "TeacherLName": "Cummings"}, ...]
        ///  
        /// GET api/TeacherData/ListTeachers/cody = >
        /// {"EmployeeNumber": "T403", "HireDate": "2016-06-13", "Salary": 43.20, "TeacherFName": "Cody", "TeacherID": 9, "TeacherLName": "Holland"}
        /// 
        /// GET api/TeacherData/ListTeachers/morris
        /// {"EmployeeNumber": "T389", "HireDate": "2012-06-04", "Salary": 48.62, "TeacherFName": "Jessica", "TeacherID": 5, "TeacherLName": "Morris"}
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{TeacherSeacrhKey?}")]
        public List<Teacher> ListTeachers(string TeacherSeacrhKey = null)
        {
            //connect to the school database
            MySqlConnection Conn = School.AccessDatabase();

            //open a connection to the database
            Conn.Open();

            //set up a string query 
            string query = "select * from teachers where teacherfname like @key or teacherlname like @key or concat(teacherfname, ' ', teacherlname) like @key";

            //create a sql command
            MySqlCommand cmd = Conn.CreateCommand();

            //execute the sql command
            cmd.CommandText = query;

            //sanitizing the teahcer  search key input
            cmd.Parameters.AddWithValue("@key", "%" + TeacherSeacrhKey + "%");

            //binding with the @key
            cmd.Prepare();

            //store the result from the query in a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //an empty list of the teacher
            List<Teacher> Teachers = new List<Teacher> { };

            //loop through the result set
            while (ResultSet.Read())
            {
                //store in varbales the values from the DB collected
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);

                string TeacherFName = ResultSet["teacherfname"].ToString();

                string TeacherLName = ResultSet["teacherlname"].ToString();

                string TeacherEmployeeNum = ResultSet["employeenumber"].ToString();

                DateTime HireDate = ResultSet.GetDateTime("hiredate");

                decimal TeacherSalary = decimal.Parse(ResultSet["salary"].ToString());

                //create an instance of Teacher
                Teacher NewTeacher = new Teacher();

                //assign to the properties the values
                NewTeacher.TeacherID = TeacherId;
                NewTeacher.TeacherFName = TeacherFName;
                NewTeacher.TeacherLName = TeacherLName;
                NewTeacher.EmployeeNumber = TeacherEmployeeNum;
                NewTeacher.HireDate = HireDate.ToString("yyyy-MM-dd");
                NewTeacher.Salary = TeacherSalary;

                //add it to the Teachers list
                Teachers.Add(NewTeacher);
            }

            //close the connection 
            Conn.Close();

            //return the list of the teachers 
            return Teachers;
        }

        /// <summary>
        /// This GET method returns an individual teacher from the database by specifying the primary key teacherid
        /// </summary>
        /// <example>GET api/TeacherData/FindTeacher/4</example>
        /// <param name="TeacherId">the teacher ID which the user is looking for</param>
        /// <returns>
        /// A teacher object which is the object that represent a teacher of the School DB
        /// GET api/TeacherData/FindTeacher/4 => {"EmployeeNumber": "T385", "HireDate": "2014-06-22", "Salary": 74.20, "TeacherFName": "Lauren", "TeacherID": 4, "TeacherLName": "Smith"}
        /// GET api/TeacherData/FindTeacher/7 => {"EmployeeNumber": "T397", "HireDate": "2013-08-04", "Salary": 64.70, "TeacherFName": "Shannon", "TeacherID": 7, "TeacherLName": "Barton"}
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{TeacherId}")]
        public Teacher FindTeacher(int TeacherId)
        {
            //connect to the school database
            MySqlConnection Conn = School.AccessDatabase();

            //open a connection to the database
            Conn.Open();

            //create a sql command
            MySqlCommand cmd = Conn.CreateCommand();

            //execute the sql command
            cmd.CommandText = "select * from teachers where teacherid = @key";

            //sanitizing the teacher find
            cmd.Parameters.AddWithValue("@key", TeacherId.ToString());

            //binding with the @key
            cmd.Prepare();

            // create an instance of Teacher
            Teacher selectedTeacher = new Teacher();

            //store the result from the query in a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //loop through the result set (it should be 1)
            while (ResultSet.Read())
            {
                //asign the values to the properties of the object
                selectedTeacher.TeacherID = Convert.ToInt32(ResultSet["teacherid"]);
                selectedTeacher.TeacherFName = ResultSet["teacherfname"].ToString();
                selectedTeacher.TeacherLName = ResultSet["teacherlname"].ToString();
                selectedTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
                selectedTeacher.HireDate = ResultSet.GetDateTime("hiredate").ToString("yyyy-MM-dd");
                selectedTeacher.Salary = decimal.Parse(ResultSet["salary"].ToString());
            }

            //close the connection 
            Conn.Close();

            //return the selected Teacher 
            return selectedTeacher;
        }


        /// <summary>
        /// This method receives the Teacger ID and delete it from the DB
        /// </summary>
        /// <param name="TeacherId">This is the ID that it's sent and it will be used on the query in order to delete the record of that specific ID</param>
        /// <example>
        /// POST: /api/TeacherData/DeleteTeacher/4
        /// This method will not return anything, it will delete a teacher from the DB</example>
        [HttpPost]
        [Route("api/TeacherData/DeleteTeacher/{TeacherId}")]
        public void DeleteTeacher(int TeacherId)
        {

            //connect to the school database
            MySqlConnection Conn = School.AccessDatabase();

            //open a connection to the database
            Conn.Open();

            //create a sql command
            MySqlCommand cmd = Conn.CreateCommand();

            //execute the sql command
            cmd.CommandText = "Delete from teachers where teacherid = @id";

            //sanitizing
            cmd.Parameters.AddWithValue("@id", TeacherId.ToString());

            //binding with the @id
            cmd.Prepare();

            //method to execute a not SELECT statement 
            cmd.ExecuteNonQuery();

            //close the connection 
            Conn.Close();
        }

        /// <summary>
        /// This method receives a Teacher and it will insert it into the database, but first it needs to aprove the validation
        /// if the validation is true the code will run completly and it will insert the teacher into the teachers table of the DB, 
        /// after that the web page will navigate the user to the /Teacher/List where you can see the new Teacher at the bottom of the List.
        /// 
        /// If the validation is false (incorrect), the code will NOT insert the tEacher int the DB and the webpage will navigate to the 
        /// /Teacher/List but in this case the Teacher will no be inserted, therefore the new teacher will NOT be seen
        /// </summary>
        /// <param name="NewTeacher">THe Teacher object that was given by the form in /Teacher/New</param>
        /// <return>
        /// null
        /// </return>
        /// <example>
        ///     POST /api/TeacherData/AddTeacher
        ///     FORM DATA:
        ///     {"TeacherFName": "Arnulfo", "TeacherLName": "Sanchez", "EmployeeNumber": "T200", "HireDate": "2023-12-01", "Salary": 35.75}
        /// </example>
        /// <example>
        ///     POST /api/TeacherData/AddTeacher
        ///     FORM DATA:
        ///     {"TeacherFName": "David", "TeacherLName": "Pena", "EmployeeNumber": "T305", "HireDate": "2023-11-15", "Salary": 55.75}
        /// 
        /// {\"TeacherFName\": \"Arnulfo\", \"TeacherLName\": \"Sanchez\", \"EmployeeNumber\": \"T200\", \"HireDate\": \"2023-12-01\", \"Salary\": 35.75}
        /// </example>
        /// <example>
        /// if you want to use CURL
        /// curl -d "{\"TeacherFName\": \"Arnulfo\", \"TeacherLName\": \"Sanchez\", \"EmployeeNumber\": \"T200\", \"HireDate\": \"2023-12-01\", \"Salary\": 35.75}" -H "Content-Type: application/json" http://localhost:54880/api/TeacherData/AddTeacher
        /// </example>
        /// 
        /// <example>
        /// another CURL example
        /// curl -d "{\"TeacherFName\": \"David\", \"TeacherLName\": \"Pena\", \"EmployeeNumber\": \"T305\", \"HireDate\": \"2023-11-15\", \"Salary\": 55.75}" -H "Content-Type: application/json" http://localhost:54880/api/TeacherData/AddTeacher
        /// </example>
        [HttpPost]
        public void AddTeacher([FromBody]Teacher NewTeacher)
        {
            //connect to the school database
            MySqlConnection Conn = School.AccessDatabase();

            //server-side validation, this method of the NewTeacher obj checks if the inputs were provided as well as if it presents data, the definition of the method is in the Model Teacher
            //If the result is false, a new Teacher will not be created and the Teachers list will be returned, however, the entered teacher is not shown in this list since
            //its validation gave a false result.
            if (!NewTeacher.IsValid()) return;

            //open a connection to the database
            Conn.Open();

            //create a sql command
            MySqlCommand cmd = Conn.CreateCommand();

            //execute the sql command
            cmd.CommandText = "Insert into teachers(teacherfname, teacherlname, employeenumber, hiredate, salary) values(@TeacherFName, @TeacherLName, @EmployeeNumber, @HireDate, @Salary)";

            //sanitizing
            cmd.Parameters.AddWithValue("@TeacherFName", NewTeacher.TeacherFName);
            cmd.Parameters.AddWithValue("@TeacherLName", NewTeacher.TeacherLName);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", DateTime.Parse(NewTeacher.HireDate));
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);

            //binding with the @id
            cmd.Prepare();

            //method to execute a not SELECT statement 
            cmd.ExecuteNonQuery();

            //close the connection 
            Conn.Close();
        }

        /// <summary>
        /// Receives a teacher ID and the new information that will overwrite the previous information, once the information is received
        /// this method will update the specified Teacher 
        /// The information will be collected in the UpdateTeacher which is a Teacher object, this object will contain the following information:
        /// TeacherFName, TeacherLName, EmployeeNumber, Salary; these will be the new data that will be used to update the Teacher.
        /// 
        /// Additionally, as it's a public void we will not have any return
        /// </summary>
        /// <param name="TeacherId">The primary key (TeacherId) to update</param>
        /// <param name="UpdateTeacher">The Teacher object, this parameter holds the new data, with this data the method will
        /// update the specified Teacher</param>
        /// <return>
        /// </return>
        /// <example>
        ///     Use of JSON OBJECT, for this we mush open the folder in the Command Prompt where our teacher.json is located
        ///     once it is open, insert the next line
        ///     curl http://localhost:54880/api/TeacherData/UpdateTeacher/18 -d @teacher.json -H "Content-Type: application/json"
        ///     In this example, the TeacherId = 18 will be updated with the information that was written in the file teacher.json
        /// </example>
        /// <example>
        ///         POST /api/TeacherData/UpdateTeacher/{TeacherId}
        ///         POST DATA:
        ///         {
        ///             "TeacherFName": "Albert", 
        ///             "TeacherLName": "Williams", 
        ///             "EmployeeNumber": "HTTP320", 
        ///             "Salary": 31.25
        ///         }
        /// </example>

        [HttpPost]
        [Route("api/TeacherData/UpdateTeacher/{TeacherId}")]
        public void UpdateTeacher(int TeacherId, [FromBody] Teacher UpdateTeacher)
        {
            //testing tha it receives the information
            Debug.WriteLine("id of the Teacher" + TeacherId);
            Debug.WriteLine("Update First Name:" + UpdateTeacher.TeacherFName);
            Debug.WriteLine("Update Last Name:" + UpdateTeacher.TeacherLName);
            Debug.WriteLine("Update Employee Number:" + UpdateTeacher.EmployeeNumber);
            Debug.WriteLine("Update Salary:" + UpdateTeacher.Salary);

            //connect to the school database
            MySqlConnection Conn = School.AccessDatabase();

            //server-side Update validation. This method of the Teacher class will be in charge of verifying that the new data that will be
            //submitted are valid. If they are not, it will cause the method to terminate, which will cause the teacher to not be updated.
            if (!UpdateTeacher.IsValidUpdate()) return;

            //open a connection to the database
            Conn.Open();

            //create a sql command
            MySqlCommand cmd = Conn.CreateCommand();

            //execute the sql command
            cmd.CommandText = "update teachers set teacherfname=@TeacherFName, teacherlname=@TeacherLName, employeenumber=@EmployeeNumber, salary=@Salary where teacherid=@id";

            //sanitizing
            cmd.Parameters.AddWithValue("@TeacherFName", UpdateTeacher.TeacherFName);
            cmd.Parameters.AddWithValue("@TeacherLName", UpdateTeacher.TeacherLName);
            cmd.Parameters.AddWithValue("@EmployeeNumber", UpdateTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@Salary", UpdateTeacher.Salary);
            cmd.Parameters.AddWithValue("@id", TeacherId); //not retrieved from the post data Teacher object

            //binding
            cmd.Prepare();

            //method to execute a not SELECT statement 
            cmd.ExecuteNonQuery();

            //close the connection 
            Conn.Close();

        }
    }
}
