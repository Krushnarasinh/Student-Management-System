using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Areas.Teacher.Models;
using Project_Consume_Api.Models;
using System.Text;

namespace Project_Consume_Api.Areas.Teacher.Controllers
{
	[CheckAccess]
	[Area("Teacher")]
	public class TeacherSubjectController : Controller
	{
		private readonly HttpClient _httpClient;
		Uri baseAddress = new Uri("http://localhost:21535/api");

		public TeacherSubjectController()
		{
			_httpClient = new HttpClient ();
			_httpClient.BaseAddress = baseAddress;
		}

		public async Task<IActionResult> TeacherSubjectList()
		{
			var response = await _httpClient.GetAsync("api/TeacherSubject");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var teacherSubjects = JsonConvert.DeserializeObject<List<TeacherSubjectModel>>(data);
				return View(teacherSubjects);
			}
			return View(new List<TeacherSubjectModel>());
		}

		public async Task<IActionResult> TeacherSubjectForm(int? TeacherSubjectID)
		{
			if (TeacherSubjectID.HasValue)
			{
				var response = await _httpClient.GetAsync($"api/TeacherSubject/{TeacherSubjectID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var teacherSubject = JsonConvert.DeserializeObject<TeacherSubjectModel>(data);
					return View(teacherSubject);
				}
			}
			return View(new TeacherSubjectModel());
		}

		public async Task<IActionResult> Save(TeacherSubjectModel teacherSubject)
		{
			if (ModelState.IsValid)
			{
				var teacherSubjectData = new
				{
					teacherSubject.TeacherSubjectID,
					teacherSubject.TeacherSubjectName,
					teacherSubject.SubjectID,
					teacherSubject.TeacherID
				};
				var json = JsonConvert.SerializeObject(teacherSubjectData);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response;

				if (teacherSubject.TeacherSubjectID == 0 || teacherSubject.TeacherSubjectID == null)
				{
					response = await _httpClient.PostAsync("api/TeacherSubject", content);
				}
				else
				{
					response = await _httpClient.PutAsync($"api/TeacherSubject/{teacherSubject.TeacherSubjectID}", content);
				}

				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("TeacherSubjectList");
				}
			}
			return View("TeacherSubjectForm", teacherSubject);
		}

		public async Task<IActionResult> Delete(int TeacherSubjectID)
		{
			var response = await _httpClient.DeleteAsync($"api/TeacherSubject/{TeacherSubjectID}");
			return RedirectToAction("TeacherSubjectList");
		}
	}
}
