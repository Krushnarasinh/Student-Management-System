
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Areas.Teacher.Models;
using System.Text;

namespace Project_Consume_Api.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class TeacherController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7266/api");
        private readonly HttpClient _client;

        public TeacherController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        public IActionResult TeacherList()
        {
            List<TeacherModel> teachers = new List<TeacherModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Teacher").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(data);
                teachers = JsonConvert.DeserializeObject<List<TeacherModel>>(data);
            }
            return View("TeacherList", teachers);
        }

        public async Task<IActionResult> TeacherForm(int? TeacherID)
        {
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
                    classID = teacher.ClassID
                };
                var json = JsonConvert.SerializeObject(teacherData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (teacher.TeacherID == 0 || teacher.TeacherID == null)
                {
                    response = await _client.PostAsync($"{_client.BaseAddress}/Teacher", content);
                }
                else
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/Teacher/{teacher.TeacherID}", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("TeacherList");
                }
            }
            return View("TeacherForm", teacher);
        }

        public async Task<IActionResult> Delete(int TeacherID)
        {
            var response = await _client.DeleteAsync($"api/Teacher/{TeacherID}");
            return RedirectToAction("TeacherList");
        }
    }
}