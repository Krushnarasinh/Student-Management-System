using Project_Consume_Api.Areas.Student.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Areas.Student.Models;
using System.Text;

namespace Project_Consume_Api.Areas.Student.Controllers
{
	[Area("Student")]
	public class StudentController : Controller
	{
		Uri baseAddress = new Uri("http://localhost:21535/api");
		private readonly HttpClient _client;

		public StudentController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
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
			if (StudentID.HasValue)
			{
				var response = await _client.GetAsync($"api/Student/{StudentID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var student = JsonConvert.DeserializeObject<StudentModel>(data.ToString());
					return View(student);
				}
			}
			return View(new StudentModel());
		}

		[HttpPost]
		public async Task<IActionResult> Save(StudentModel student)
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
				}
				else
				{
					response = await _client.PutAsync($"{_client.BaseAddress}/Student/{student.StudentID}", content);
				}

				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("StudentList");
				}
			}
			return View("StudentForm", student);
		}

		public async Task<IActionResult> Delete(int StudentID)
		{
			var response = await _client.DeleteAsync($"api/Student/{StudentID}");
			return RedirectToAction("StudentList");
		}

		public IActionResult StudentHome()
		{
			return View();
		}

	}
}