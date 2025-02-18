using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Areas.Student.Models;
using Project_Consume_Api.Areas.Teacher.Models;
using Project_Consume_Api.Models;
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

        public async Task<IActionResult> StudentList()
        {
            List<StudentModel> students = new List<StudentModel>();
            HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/Student");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                Console.WriteLine(data);
                students = JsonConvert.DeserializeObject<List<StudentModel>>(data);
            }
            return View("StudentList", students);
        }

       

        public async Task<IActionResult> Delete(int StudentID)
        {
            var response = await _client.DeleteAsync($"api/Student/{StudentID}");
            TempData["StudentDeleted"] = response.IsSuccessStatusCode ? "Student deleted successfully!" : "Foreign Key Error: Student deletion failed!";
            return RedirectToAction("StudentList");
        }

        private async Task LoadClassList()
        {
            var response = await _client.GetAsync("api/Student/Classes");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var classes = JsonConvert.DeserializeObject<List<classdropdownmodel>>(data);
                ViewBag.ClassList = classes;
            }
        }

        public async Task<IActionResult> StudentHome()
        {
            string studentID = HttpContext.Session.GetString("StudentID");

            if (studentID == null)
            {
                return Json(new { error = "No StudentID found in session" });
            }

            var response = await _client.GetAsync($"api/Teacher/GetStudentDetails/{studentID}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();

                return View();
            }

            return View();

        }

        public async Task<IActionResult> StudentAttendance()
        {
            HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/Teacher/GetStudentAttendance/{CommonVariable.StudentID()}");

            List<AttendanceModel> attendances = new List<AttendanceModel>();

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                attendances = JsonConvert.DeserializeObject<List<AttendanceModel>>(data);
                return View(attendances);
            }

            TempData["Error"] = "Failed to fetch student attendance.";
            return View(attendances);
        }

        public async Task<IActionResult> StudentGrades()
        {
            HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/Teacher/GetStudentGrades/{CommonVariable.StudentID()}");
            List<GradeModel> grades = new List<GradeModel>();

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                grades = JsonConvert.DeserializeObject<List<GradeModel>>(data);
                return View(grades);
            }

            TempData["Error"] = "Failed to fetch student grades.";
            return View(grades);
        }

    }
}
