using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Areas.Teacher.Models;
using Project_Consume_Api.Models;
using System.Text;

namespace Project_Consume_Api.Areas.Teacher.Controllers
{
	[CheckAccess]
	[Area("Teacher")]
	public class AttendanceController : Controller
	{
		private readonly HttpClient _httpClient;
		 Uri baseAddress = new Uri("http://localhost:21535/api");

		public AttendanceController()
		{
			_httpClient = new HttpClient ();
			_httpClient.BaseAddress = baseAddress;
		}

		public async Task<IActionResult> AttendanceList()
		{
			var response = await _httpClient.GetAsync("api/Attendance");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var attendances = JsonConvert.DeserializeObject<List<AttendanceModel>>(data);
				return View(attendances);
			}
			return View(new List<AttendanceModel>());
		}

		public async Task<IActionResult> AttendanceForm(int? AttendanceID)
		{
			if (AttendanceID.HasValue)
			{
				var response = await _httpClient.GetAsync($"api/Attendance/{AttendanceID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var attendance = JsonConvert.DeserializeObject<AttendanceModel>(data);
					return View(attendance);
				}
			}
			return View(new AttendanceModel());
		}

		public async Task<IActionResult> Save(AttendanceModel attendance)
		{
			if (ModelState.IsValid)
			{
				var attendanceData = new
				{
					attendance.AttendanceID,
					attendance.Status,
					attendance.TeacherID,
					attendance.StudentID,
					attendance.Date
				};
				var json = JsonConvert.SerializeObject(attendanceData);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response;

				if (attendance.AttendanceID == 0 || attendance.AttendanceID == null)
				{
					response = await _httpClient.PostAsync("api/Attendance", content);
				}
				else
				{
					response = await _httpClient.PutAsync($"api/Attendance/{attendance.AttendanceID}", content);
				}

				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("AttendanceList");
				}
			}
			return View("AttendanceForm", attendance);
		}

		public async Task<IActionResult> Delete(int AttendanceID)
		{
			var response = await _httpClient.DeleteAsync($"api/Attendance/{AttendanceID}");
			return RedirectToAction("AttendanceList");
		}
	}
}
