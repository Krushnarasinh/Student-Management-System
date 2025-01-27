using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Areas.Teacher.Models;
using System.Text;

namespace Project_Consume_Api.Areas.Teacher.Controllers
{
	[CheckAccess]
	[Area("Teacher")]
	public class GradeController : Controller
	{
		private readonly HttpClient _httpClient;
		Uri baseAddress = new Uri("http://localhost:21535/api");

		public GradeController()
		{
			_httpClient = new HttpClient ();
			_httpClient.BaseAddress = baseAddress;
		}

		public async Task<IActionResult> GradeList()
		{
			var response = await _httpClient.GetAsync("api/Grade");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var grades = JsonConvert.DeserializeObject<List<GradeModel>>(data);
				return View(grades);
			}
			return View(new List<GradeModel>());
		}

		public async Task<IActionResult> GradeForm(int? GradeID)
		{
			if (GradeID.HasValue)
			{
				var response = await _httpClient.GetAsync($"api/Grade/{GradeID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var grade = JsonConvert.DeserializeObject<GradeModel>(data);
					return View(grade);
				}
			}
			return View(new GradeModel());
		}

		public async Task<IActionResult> Save(GradeModel grade)
		{
			if (ModelState.IsValid)
			{
				var gradeData = new
				{
					grade.GradeID,
					grade.StudentID,
					grade.SubjectID,
					grade.Score,
					grade.TeacherID
				};
				var json = JsonConvert.SerializeObject(gradeData);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response;

				if (grade.GradeID == 0 || grade.GradeID == null)
				{
					response = await _httpClient.PostAsync("api/Grade", content);
				}
				else
				{
					response = await _httpClient.PutAsync($"api/Grade/{grade.GradeID}", content);
				}

				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("GradeList");
				}
			}
			return View("GradeForm", grade);
		}

		public async Task<IActionResult> Delete(int GradeID)
		{
			var response = await _httpClient.DeleteAsync($"api/Grade/{GradeID}");
			return RedirectToAction("GradeList");
		}
	}
}
