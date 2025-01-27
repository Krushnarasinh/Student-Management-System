using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Models;
using System.Text;

namespace Project_Consume_Api.Areas.Teacher.Controllers
{
    [CheckAccess]
    [Area("Teacher")]
    public class ClassController : Controller
    {
        private readonly HttpClient _httpClient;
        Uri baseAddress = new Uri("http://localhost:21535/api");

        public ClassController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        public async Task<IActionResult> ClassList()
        {
            var response = await _httpClient.GetAsync("api/Classes");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var classes = JsonConvert.DeserializeObject<List<ClassModel>>(data);
                return View(classes);
            }
            return View(new List<ClassModel>());
        }

        public async Task<IActionResult> ClassForm(int? ClassID)
        {
            if (ClassID.HasValue)
            {
                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Classes/{ClassID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var classes = JsonConvert.DeserializeObject<ClassModel>(data);
                    return View(classes);
                }
            }
            return View(new ClassModel());
        }

        public async Task<IActionResult> Save(ClassModel classes)
        {
            if (ModelState.IsValid)
            {
                var classData = new
                {
                    classID = classes.ClassID,
                    className = classes.ClassName
                };
                var json = JsonConvert.SerializeObject(classData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (classes.ClassID == 0 || classes.ClassID == null)
                {
                    response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Classes", content);
                }
                else
                {
                    response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Classes/{classes.ClassID}", content);
                }
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ClassList");
                }
            }
            return View("ClassForm", classes);
        }

        public async Task<IActionResult> Delete(int ClassID)
        {
            var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Classes/{ClassID}");
            return RedirectToAction("ClassList");
        }
    }
}
