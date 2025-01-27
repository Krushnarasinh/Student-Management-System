using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api;
using Project_Consume_Api.Areas.Teacher.Models;
using System.Text;

namespace Consume_API_StudentControllers
{
	[CheckAccess]
	[Area("Teacher")]
	public class SubjectController : Controller
    {
        private readonly HttpClient _httpClient;
        Uri baseAddress = new Uri("http://localhost:21535/api");

        public SubjectController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        public async Task<IActionResult> SubjectList()
        {
            var response = await _httpClient.GetAsync("api/Subject");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var subject = JsonConvert.DeserializeObject<List<SubjectModel>>(data);
                return View(subject);
            }
            return View(new List<SubjectModel>());
        }

        public async Task<IActionResult> SubjectForm(int? SubjectID)
        {
            if (SubjectID.HasValue)
            {
                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Subject/{SubjectID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var subject = JsonConvert.DeserializeObject<SubjectModel>(data);
                    return View(subject);
                }
            }
            return View(new SubjectModel());
        }

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
                    response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Subject", content);
                }
                else
                {
                    response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Subject/{subject.SubjectID}", content);
                }
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("SubjectList");
                }
            }
            return View("SubjectForm", subject);
        }

        public async Task<IActionResult> Delete(int SubjectID)
        {
            var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Subject/{SubjectID}");
            return RedirectToAction("SubjectList");
        }
    }
}
