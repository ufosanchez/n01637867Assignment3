function UpdateTeacher(id) {

	//goal: send a request which looks like this:
	//POST : http://localhost:54880/api/TeacherData/UpdateTeacher/18
	//with POST data of the information collected.

	var URL = "/api/TeacherData/UpdateTeacher/" + id;

	//use XMLHttpRequest object
	var rq = new XMLHttpRequest();

	//retrieve informtion from inputs
	var TeacherFName = document.getElementById('TeacherFName').value;
	var TeacherLName = document.getElementById('TeacherLName').value;
	var EmployeeNumber = document.getElementById('EmployeeNumber').value;
	var Salary = document.getElementById('Salary').value;

	var error = document.getElementById("errorMsg");

	//create object that defines the update teacher
	var TeacherUpdateData = {
		"TeacherFName": TeacherFName,
		"TeacherLName": TeacherLName,
		"EmployeeNumber": EmployeeNumber,
		"Salary": Salary
	};

	if (TeacherUpdateData.TeacherFName === "" || TeacherUpdateData.TeacherLName === "" || TeacherUpdateData.EmployeeNumber === "" || TeacherUpdateData.Salary === "") {

		error.innerHTML = "ERROR - Please fill in all the inputs";

	} else {

		error.innerHTML = "";

		//POST request api/TeacherData/UpdateTeacher/{TeacherId}
		rq.open("POST", URL, true);
		rq.setRequestHeader("Content-Type", "application/json");
		rq.onreadystatechange = function () {
			//ready state should be 4, status should be 200, I added the status 204 because server successfully processes the request, but there is no content to return to the client
			if (rq.readyState === 4 && rq.status === 200 || rq.status === 204) {
				//request is successful and the request is finished

				window.location.href = "/Teacher/Show/" + id;

			} else {
				console.log(rq.status);
			}

		}
		//POST information sent through the .send() method
		rq.send(JSON.stringify(TeacherUpdateData));
		console.log("the Teacher was updated")
	}
}