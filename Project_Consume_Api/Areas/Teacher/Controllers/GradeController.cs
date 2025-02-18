using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Areas.Student.Models;
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
            List<GradeModel> grades = new List<GradeModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Grade").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(data);
                grades = JsonConvert.DeserializeObject<List<GradeModel>>(data);
            }
            return View("GradeList", grades);
        }


		public async Task<IActionResult> GradeForm(int? GradeID)
		{
            await LoadTeacherList();
            await LoadStudentList();
            await LoadSubjectList();
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
        [HttpPost]
        public async Task<IActionResult> Save(List<GradeModel> grade, bool isAdd, int subjectID)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(grade);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (isAdd)
                {
                    response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Grade", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["GradeInsert"] = "Grade added successfully!";
                    }
                    else
                    {
                        TempData["GradeInsert"] = "Failed to add the Grade.";
                    }
                }
                else
                {
                    response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Grade", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["GradeUpdated"] = "Grade updated successfully!";
                    }
                    else
                    {
                        TempData["GradeUpdated"] = "Failed to update the Grade.";
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ClassList", new { subjectID = subjectID });
                }
            }
            return View("GradeForm", grade);
        }

        public async Task<IActionResult> Delete(int GradeID)
        {
            var response = await _httpClient.DeleteAsync($"api/Grade/{GradeID}");
            if (response.IsSuccessStatusCode)
            {
                TempData["GradeDeleted"] = "Grade deleted successfully!";
            }
            else
            {
                TempData["GradeNotDeleted"] = "Foreign Key Error Grade deletion Failed!";
            }
            return RedirectToAction("GradeList");
        }
        private async Task LoadTeacherList()
        {
            var response = await _httpClient.GetAsync("api/Grade/Teacher");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var teachers = JsonConvert.DeserializeObject<List<ForGradeTeacherDropDownModel>>(data);
                ViewBag.TeacherList = teachers;
            }
        }

        private async Task LoadStudentList()
        {
            var response = await _httpClient.GetAsync("api/Grade/Student");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var students = JsonConvert.DeserializeObject<List<ForGradeStudentDropDownModel>>(data);
                ViewBag.StudentList = students;
            }
        }

        private async Task LoadSubjectList()
        {
            var response = await _httpClient.GetAsync("api/Grade/Subject");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var subjects = JsonConvert.DeserializeObject<List<ForGradeSubjectDropDownModel>>(data);
                ViewBag.SubjectList = subjects;
            }
        }

        public async Task<IActionResult> ClassList(int subjectID)
        {
            var response = await _httpClient.GetAsync($"api/Grade/GetClassList?SubjectID={subjectID}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var classes = JsonConvert.DeserializeObject<List<GradeClassListModel>>(data);
                return View(classes);
            }
            return View(new List<GradeClassListModel>());
        }

        public IActionResult StudentList(int classId, int subjectId)
        {
            List<StudentModel> students = new List<StudentModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Student?subjectId={subjectId}&classId={classId}").Result;

            List<GradeModel> grades = new List<GradeModel>();


            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                students = JsonConvert.DeserializeObject<List<StudentModel>>(data);

                for (int i = 0; i < students.Count; i++)
                {
                    grades.Add(new GradeModel() { ClassID = classId, Score = 0, TeacherID = CommonVariable.TeacherID() ?? 0, StudentID = subjectId, SubjectName = students[i].StudentName }
                        );
                }
            }
            return View("StudentList", students);
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

        public async Task<IActionResult> FillOrEditGrade(bool? isAdd, int classID, int subjectID)
        {
            List<GradeModel> grades = new List<GradeModel>();

            if (isAdd ?? true)
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Student?subjectId={subjectID}&classId={classID}");

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    grades = JsonConvert.DeserializeObject<List<GradeModel>>(data);

                    for (int i = 0; i < grades.Count; i++)
                    {
                        grades[i] = new GradeModel()
                        {
                            GradeID = grades[i].GradeID,
                            TeacherID = CommonVariable.TeacherID() ?? 0,
                            Score = grades[i].Score,
                            StudentID = grades[i].StudentID,
                            ClassID = classID,
                            StudentName = grades[i].StudentName,
                            RollNo = grades[i].RollNo,
                            SubjectID = subjectID,
                        };
                    }
                }
            }
            else
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Grade/GetGradeListByClassId?classID={classID}&subjectID={subjectID}");

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    grades = JsonConvert.DeserializeObject<List<GradeModel>>(data);
                }
            }
            return View(grades);
        }

        [HttpPost]
        public async Task<IActionResult> FillOrEditGrade(List<GradeModel> grades, bool? isAdd, int subjectID)
        {
            if (grades == null || !grades.Any())
            {
                return RedirectToAction("ClassList");
            }


            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(grades);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                if (isAdd ?? true)
                {
                    // fill
                    HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Grade", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["GradeInsert"] = "Grade added successfully!";
                    }
                    else
                    {
                        TempData["GradeInsert"] = "Failed to add the Grade.";
                    }
                }
                else
                {
                    //edit

                    HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Grade/UpdateGrade", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["GradeUpdated"] = "Grade Edit successfully!";
                    }
                    else
                    {
                        TempData["GradeUpdated"] = "Failed to Edit the Grade.";
                    }
                }   
            }

            return RedirectToAction("ClassList", new { subjectID = subjectID });


        }
    }
}
