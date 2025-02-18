using Project_Consume_Api.Areas.Student.Models;
using Project_Consume_Api.Areas.Teacher.Models;
using Project_Consume_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Project_Consume_Api.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class AuthController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:21535/api");
        private readonly HttpClient _httpClient;
        private static IHttpContextAccessor _HttpContextAccessor;

        public AuthController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
            _HttpContextAccessor = new HttpContextAccessor();
        }
        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginModel logIn)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        HttpResponseMessage response;
        //        if (logIn.Role == "Student")
        //        {
        //            var json = JsonConvert.SerializeObject(logIn);
        //            var content = new StringContent(json, Encoding.UTF8, "application/json");
        //            response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Student/Login", content);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                StudentModel student = JsonConvert.DeserializeObject<StudentModel>(data.ToString());

        //                _HttpContextAccessor.HttpContext.Session.SetString("StudentID", student.StudentID.ToString());
        //                _HttpContextAccessor.HttpContext.Session.SetString("EmailAddress", student.EmailID.ToString());
        //                return RedirectToAction("StudentHome", "Student", new { area = "Student" });
        //            }
        //        }
        //        else
        //        {
        //            //tea
        //            var json = JsonConvert.SerializeObject(logIn);
        //            var content = new StringContent(json, Encoding.UTF8, "application/json");
        //            response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Teacher/Login", content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                TeacherModel teacher = JsonConvert.DeserializeObject<TeacherModel>(data.ToString());

        //                _HttpContextAccessor.HttpContext.Session.SetString("TeacherID", teacher.TeacherID.ToString());
        //                _HttpContextAccessor.HttpContext.Session.SetString("EmailAddress", teacher.EmailID.ToString());
        //                return RedirectToAction("ClassList", "Class");
        //            }
        //        }
        //    }
        //    return View(logIn);
        //}

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel logIn)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response;

                if (logIn.Role == "Student")
                {
                    var json = JsonConvert.SerializeObject(logIn);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Student/Login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        StudentLogInModel studentLogIn = JsonConvert.DeserializeObject<StudentLogInModel>(data.ToString());

                        StudentModel student = studentLogIn.student;
                        string token = studentLogIn.token;

                        _HttpContextAccessor.HttpContext.Session.SetString("StudentID", student.StudentID.ToString());
                        _HttpContextAccessor.HttpContext.Session.SetString("EmailAddress", student.EmailID.ToString());

                        CommonVariable.SetToken(token);
                        return RedirectToAction("Index1", "StudentDashBoard", new { area = "Student" });
                        }
                    }
                
                else if (logIn.Role == "Teacher")
                {
                    var json = JsonConvert.SerializeObject(logIn);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Teacher/Login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        TeacherLoginResponse teacherLogin = JsonConvert.DeserializeObject<TeacherLoginResponse>(data.ToString());

                        TeacherModel teacher = teacherLogin.Teacher;

                        _HttpContextAccessor.HttpContext.Session.SetString("TeacherID", teacher.TeacherID.ToString());
                        _HttpContextAccessor.HttpContext.Session.SetString("EmailAddress", teacher.EmailID.ToString());

                        CommonVariable.SetToken(teacherLogin.Token);
                        return RedirectToAction("Index", "TeacherDashBoard");
                        }
                    }
                

                // Set an error message if login fails
                ViewBag.ErrorMessage = "Invalid login credentials. Please try again.";
            }

            return View(logIn);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            var userType = HttpContext.Session.GetString("UserType");

            if (userType == "Student")
            {
                return RedirectToAction("Login", "Student");
            }
            else if (userType == "Teacher")
            {
                return RedirectToAction("Login", "Teacher");
            }

            return RedirectToAction("Login", "Auth");
        }

    }
}
