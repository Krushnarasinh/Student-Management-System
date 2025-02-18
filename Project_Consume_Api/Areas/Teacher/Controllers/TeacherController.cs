
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Areas.Student.Models;
using Project_Consume_Api.Areas.Teacher.Models;
using System.Text;
using System.Net.Http;
using Project_Consume_Api.Models;

namespace Project_Consume_Api.Areas.Teacher.Controllers
{
	[CheckAccess]
	[Area("Teacher")]
    public class TeacherController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:21535/api");
        private readonly HttpClient _client;

        public TeacherController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonVariable.GetToken());
        }

        public async Task<IActionResult> TeacherList()
        {
            List<TeacherModel> teachers = new List<TeacherModel>();
            HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/Teacher");

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(data);
                teachers = JsonConvert.DeserializeObject<List<TeacherModel>>(data);
            }
            return View("TeacherList", teachers);
        }
        public async Task<IActionResult> TeacherProfile()
        {
            var response = await _client.GetAsync($"api/Teacher/{CommonVariable.TeacherID()}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var teacher = JsonConvert.DeserializeObject<TeacherModel>(data);
                return View("TeacherProfile", teacher);
            }
            TempData["ErrorMessage"] = "Teacher not found!";
            return RedirectToAction("TeacherList");
        }




        public async Task<IActionResult> TeacherForm(int? TeacherID)
        {
            await LoadClassList();
            if (TeacherID.HasValue)
            {
                var response = await _client.GetAsync($"api/Teacher/{TeacherID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var teacher = JsonConvert.DeserializeObject<TeacherModel>(data.ToString());
                    return View(teacher);
                }
            }
            return View(new TeacherModel());
        }

        [HttpPost]
        public async Task<IActionResult> Save(TeacherModel teacher)
        {
            if (ModelState.IsValid)
            {
                var teacherData = new
                {
                    teacherID = teacher.TeacherID,
                    teacherName = teacher.TeacherName,
                    contactInfo = teacher.ContactInfo,
                    emailID = teacher.EmailID,
                    password = teacher.Password,
                    classID = teacher.ClassID,
					className = teacher.ClassName
				};
                var json = JsonConvert.SerializeObject(teacherData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (teacher.TeacherID == 0 || teacher.TeacherID == null)
                {

                    response = await _client.PostAsync($"{_client.BaseAddress}/Teacher", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["TeacherInsert"] = "Teacher added successfully!";
                    }
                    else
                    {
                        TempData["TeacherInsert"] = "Failed to Teacher the Grade.";
                    }
                }
                else
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/Teacher/{teacher.TeacherID}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["TeacherUpdated"] = "Teacher updated successfully!";
                    }
                    else
                    {
                        TempData["TeacherUpdated"] = "Failed to update the Teacher.";
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("TeacherList");
                }

            }
            await LoadClassList();
            return View("TeacherForm", teacher);
        }

		//public async Task<IActionResult> Delete(int TeacherID)
		//{
		//    var response = await _client.DeleteAsync($"api/Teacher/{TeacherID}");
		//    return RedirectToAction("TeacherList");
		//}
		public async Task<IActionResult> Delete(int TeacherID)
		{
			var response = await _client.DeleteAsync($"api/Teacher/{TeacherID}");
			if (response.IsSuccessStatusCode)
			{
				TempData["TeacherDeleted"] = "Teacher deleted successfully!";
			}
			else
			{
				TempData["TeacherNotDeleted"] = "Foreign Key Error Teacher deletion Failed!";
			}
			return RedirectToAction("TeacherList");
		}
		private async Task LoadClassList()
		{
			var response = await _client.GetAsync("api/Teacher/Classes");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var classes = JsonConvert.DeserializeObject<List<classdropdownmodel>>(data);
				ViewBag.ClassList = classes;
			}
		}

		public IActionResult StudentList()
		{
			List<StudentModel> students = new List<StudentModel>();
			HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Student").Result;

			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				Console.WriteLine(data);
				students = JsonConvert.DeserializeObject<List<StudentModel>>(data);
			}
			return View("StudentList", students);
		}

        public async Task<IActionResult> StudentForm(int? StudentID)
        {
            await LoadClassList(); // Load Class Dropdown
            if (StudentID.HasValue)
            {
                var response = await _client.GetAsync($"api/Student/{StudentID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var student = JsonConvert.DeserializeObject<StudentModel>(data);
                    return View(student);
                }
            }
            return View(new StudentModel());
        }

        [HttpPost]
        public async Task<IActionResult> StudentForm(StudentModel student)
        {
            if (ModelState.IsValid)
            {
                var studentData = new
                {
                    studentID = student.StudentID,
                    studentName = student.StudentName,
                    dob = student.DOB,
                    gender = student.Gender,
                    contactInfo = student.ContactInfo,
                    rollNo = student.RollNo,
                    emailID = student.EmailID,
                    password = student.Password,
                    classID = student.ClassID
                };

                var json = JsonConvert.SerializeObject(studentData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (student.StudentID == 0 || student.StudentID == null)
                {
                    response = await _client.PostAsync($"{_client.BaseAddress}/Student", content);
                    TempData["StudentInsert"] = response.IsSuccessStatusCode ? "Student added successfully!" : "Failed to add student.";
                }
                else
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/Student/{student.StudentID}", content);
                    TempData["StudentUpdated"] = response.IsSuccessStatusCode ? "Student updated successfully!" : "Failed to update student.";
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("StudentList");
                }
            }
            await LoadClassList();
            return View("StudentForm", student);
        }
        public async Task<IActionResult> StudentDelete(int StudentID)
        {
            var response = await _client.DeleteAsync($"api/Student/{StudentID}");
            TempData["StudentDeleted"] = response.IsSuccessStatusCode ? "Student deleted successfully!" : "Foreign Key Error: Student deletion failed!";
            return RedirectToAction("StudentList");
        }
    }
}