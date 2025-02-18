using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Consume_Api.Areas.Student.Models;
using Project_Consume_Api.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Project_Consume_Api.Areas.Student.Controllers
{
    [CheckAccess]
    [Area("Student")]
    public class StudentDashBoardController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:21535/api");
        private readonly HttpClient _client;

        public StudentDashBoardController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        /// <summary>
        /// Gets the student dashboard data including counts, student details, attendance records, and grade records.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>Returns the complete student dashboard data as JSON.</returns>
        public async Task<IActionResult> Index1()
        {
            StudentDashBoardModel dashboardData = new StudentDashBoardModel
            {
                Counts = new List<StudentDashboardCounts>(),
                AttendanceRecords = new List<AttendanceInfo>(),
                GradeRecords = new List<GradeInfo>()
            };

            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/StudentDashBoard/GetDashboardData/{CommonVariable.StudentID()}");
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    dashboardData = JsonConvert.DeserializeObject<StudentDashBoardModel>(jsonResponse);
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to load student dashboard data.";
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
