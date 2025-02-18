using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Areas.Teacher.Models;
using Project_Consume_Api.Models;
using System.Text;
using System.Net.Http;
using Project_Consume_Api;

namespace Consume_API_StudentControllers
{
    [CheckAccess]
    [Area("Teacher")]
    public class SubjectController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:21535/api");
        private readonly HttpClient _httpClient;

        public SubjectController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        // Subject List
        public async Task<IActionResult> SubjectList()
        {
            var response = await _httpClient.GetAsync("api/Subject");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var subjects = JsonConvert.DeserializeObject<List<SubjectModel>>(data);
                return View(subjects);
            }
            return View(new List<SubjectModel>());
        }

        // Subject Form (Add/Edit)
        public async Task<IActionResult> SubjectForm(int? SubjectID)
        {
            if (SubjectID.HasValue)
            {
                var response = await _httpClient.GetAsync($"api/Subject/{SubjectID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var subject = JsonConvert.DeserializeObject<SubjectModel>(data);
                    return View(subject);
                }
            }
            return View(new SubjectModel());
        }

        // Save Subject (Add/Update)
        [HttpPost]
        public async Task<IActionResult> Save(SubjectModel subject)
        {
            if (ModelState.IsValid)
            {
                var subjectData = new
                {
                    subjectID = subject.SubjectID,
                    subjectName = subject.SubjectName
                };
                var json = JsonConvert.SerializeObject(subjectData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (subject.SubjectID == 0 || subject.SubjectID == null)
                {
                    // Add new subject
                    response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Subject", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SubjectInsert"] = "Subject added successfully!";
                    }
                    else
                    {
                        TempData["SubjectInsert"] = "Failed to add the subject.";
                    }
                }
                else
                {
                    // Update existing subject
                    response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Subject/{subject.SubjectID}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SubjectUpdated"] = "Subject updated successfully!";
                    }
                    else
                    {
                        TempData["SubjectUpdated"] = "Failed to update the subject.";
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("SubjectList");
                }
            }
            return View("SubjectForm", subject);
        }

        // Delete Subject
        public async Task<IActionResult> Delete(int SubjectID)
        {
            var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Subject/{SubjectID}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SubjectDeleted"] = "Subject deleted successfully!";
            }
            else
            {
                TempData["SubjectNotDeleted"] = "Failed to delete the subject due to a foreign key error!";
            }
            return RedirectToAction("SubjectList");
        }
    }
}
