using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Models;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace Project_Consume_Api.Areas.Teacher.Controllers
{
    [CheckAccess]
    [Area("Teacher")]
    public class ClassController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:21535/api");
        private readonly HttpClient _httpClient;

        public ClassController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        // Method to fetch and display the list of classes
        public async Task<IActionResult> ClassList()
        {
            List<ClassModel> classes = new List<ClassModel>();
            var response = await _httpClient.GetAsync("api/Classes");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                classes = JsonConvert.DeserializeObject<List<ClassModel>>(data);
            }

            return View("ClassList", classes);
        }

        // Method to fetch data and display form for adding/editing a class
        public async Task<IActionResult> ClassForm(int? ClassID)
        {
            if (ClassID.HasValue)
            {
                var response = await _httpClient.GetAsync($"api/Classes/{ClassID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var classData = JsonConvert.DeserializeObject<ClassModel>(data);
                    return View(classData);
                }
            }
            return View(new ClassModel());
        }

        // Method to save the class (either create or update)
        [HttpPost]
        public async Task<IActionResult> Save(ClassModel classModel)
        {
            if (ModelState.IsValid)
            {
                var classData = new
                {
                    classID = classModel.ClassID,
                    className = classModel.ClassName
                };
                var json = JsonConvert.SerializeObject(classData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (classModel.ClassID == 0 || classModel.ClassID == null)
                {
                    response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Classes", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["ClassInsert"] = "Class added successfully!";
                    }
                    else
                    {
                        TempData["ClassInsert"] = "Failed to add the class.";
                    }
                }
                else
                {
                    response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Classes/{classModel.ClassID}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["ClassUpdated"] = "Class updated successfully!";
                    }
                    else
                    {
                        TempData["ClassUpdated"] = "Failed to update the class.";
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ClassList");
                }
            }
            return View("ClassForm", classModel);
        }

        public async Task<IActionResult> Delete(int ClassID)
        {
            var response = await _httpClient.DeleteAsync($"api/Classes/{ClassID}");

            if (response.IsSuccessStatusCode)
            {
                TempData["ClassDeleted"] = "Class deleted successfully!";
            }
            else
            {
                TempData["ClassNotDeleted"] = "Foreign Key Error. Class deletion failed!";
            }

            return RedirectToAction("ClassList");
        }
    }
}
