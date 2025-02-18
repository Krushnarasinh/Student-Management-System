using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Areas.Student.Models;
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
                    attendanceID = attendance.AttendanceID,
                    status = attendance.Status,
                    teacherID = attendance.TeacherID,
                    studentID = attendance.StudentID,
                    date = attendance.Date
                };
                var json = JsonConvert.SerializeObject(attendanceData);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response;

				if (attendance.AttendanceID == 0 || attendance.AttendanceID == null)
				{
					response = await _httpClient.PostAsync("api/Attendance", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["AttendanceInsert"] = "Attendance added successfully!";
                    }
                    else
                    {
                        TempData["AttendanceInsert"] = "Failed to add the Attendance.";
                    }
                }
				else
				{
					response = await _httpClient.PutAsync($"api/Attendance/{attendance.AttendanceID}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["AttendanceUpdated"] = "Attendance updated successfully!";
                    }
                    else
                    {
                        TempData["AttendanceUpdated"] = "Failed to update the Attendance.";
                    }
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
            if (response.IsSuccessStatusCode)
            {
                TempData["AttendanceDeleted"] = "Attendance deleted successfully!";
            }
            else
            {
                TempData["AttendanceNotDeleted"] = "Foreign Key Error Attendance deletion Failed!";
            }
            return RedirectToAction("AttendanceList");
        }
        #region class List
        public async Task<IActionResult> ClassList(string date)
        {
            var response = await _httpClient.GetAsync($"api/Attendance/ClassList?date={date.ToString()}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var classes = JsonConvert.DeserializeObject<List<AttendanceListModel>>(data);
                return View(classes);
            }
            return View(new List<AttendanceListModel>());
        }
        #endregion

        #region student List
        public IActionResult StudentList()
        {
            List<StudentModel> students = new List<StudentModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Student").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(data);
                students = JsonConvert.DeserializeObject<List<StudentModel>>(data);
            }
            return View("StudentList", students);
        }
        #endregion

        #region fill Attendanse
        public async Task<IActionResult> FillOrEditAttendance(bool? isAdd, DateTime? date, int classID)
        {
            List<AttendanceModel> attendances = new List<AttendanceModel>();

            if (isAdd ?? true)
            {
                HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Student/GetStudentListByClassID/{classID}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    attendances = JsonConvert.DeserializeObject<List<AttendanceModel>>(data);

                    for (int i = 0; i < attendances.Count; i++)
                    {
                        attendances[i] = new AttendanceModel()
                        {
                            AttendanceID = attendances[i].AttendanceID,
                            TeacherID = CommonVariable.TeacherID() ?? 0,
                            Status = "Present",
                            StudentID = attendances[i].StudentID,
                            ClassID = classID,
                            StudentName = attendances[i].StudentName,
                            RollNo = attendances[i].RollNo,
                            Date =date != null ? date.ToString() : DateTime.Now.ToString(),
                        };
                    }
                }
            }
            else
            {
                HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Attendance/GetAttenndanceListByDateAndClassID?date={date}&classID={classID}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    attendances = JsonConvert.DeserializeObject<List<AttendanceModel>>(data);
                }
            }
            return View(attendances);
        }

        [HttpPost]
        public async Task<IActionResult> FillOrEditAttendance(List<AttendanceModel> attendances, bool? isAdd, string datetime)
        {
            if (attendances == null || !attendances.Any())
            {
                TempData["AttendanceInsert"] = "No attendance data provided.";
                return RedirectToAction("ClassList");
            }

            foreach (var attendance in attendances)
            {
                if (string.IsNullOrEmpty(attendance.Status))
                {
                    attendance.Status = "Absent";
                }
            }

            if (isAdd ?? true)
            {
                // fill
                // Set absent status for unchecked items


                var json = JsonConvert.SerializeObject(attendances);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Attendance/FillAttendance", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["AttendanceInsert"] = "Attendance added successfully!";
                }
                else
                {
                    TempData["AttendanceInsert"] = "Failed to add the attendance.";
                }
            }
            else
            {
                //edit
                var json = JsonConvert.SerializeObject(attendances);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Attendance/Edit", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AttendanceInsert"] = "Attendance Edit successfully!";
                }
                else
                {
                    TempData["AttendanceInsert"] = "Failed to Edit the attendance.";
                }
            }

            // Correcting the RedirectToAction syntax
            return RedirectToAction("ClassList", new { date = datetime.ToString() });

        }
        #endregion
    }
}
