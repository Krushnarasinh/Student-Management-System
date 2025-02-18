using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Areas.Teacher.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Project_Consume_Api.Areas.Teacher.Controllers
{
    [CheckAccess]
    [Area("Teacher")]
    public class TeacherDashBoardController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:21535/api");
        private readonly HttpClient _client;

        public TeacherDashBoardController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        /// <summary>
        /// Gets the teacher dashboard data including counts, recent students, recent teachers, top attendance, and top grades.
        /// </summary>
        /// <returns>Returns the complete dashboard data as JSON.</returns>
        public async Task<IActionResult> Index()
        {
            TeacherDashBoardModel dashboardData = new TeacherDashBoardModel
            {
                Counts = new List<DashboardCounts>(),
                RecentStudents = new List<RecentStudent>(),
                RecentTeachers = new List<RecentTeacher>(),
                TopAttendances = new List<TopAttendance>(),
                TopGrades = new List<TopGrade>()
            };

            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/TeacherDashBoard/GetDashboardData");
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    dashboardData = JsonConvert.DeserializeObject<TeacherDashBoardModel>(jsonResponse);
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to load dashboard data.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error: {ex.Message}";
            }

            return View(dashboardData);
        }
    }
}
