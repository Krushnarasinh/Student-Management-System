namespace Project_Consume_Api.Areas.Teacher.Models
{

    public class DashboardCounts
    {
        public string Metric { get; set; }
        public int Value { get; set; }
    }

    public class RecentStudent
    {
        public int? StudentID { get; set; }
        public string StudentName { get; set; }
        public string RollNo { get; set; }
        public string EmailID { get; set; }
        public int? ClassID { get; set; }
        public string ClassName { get; set; }
        public int? SubjectID { get; set; }
    }

    public class RecentTeacher
    {
        public int? TeacherID { get; set; }
        public string TeacherName { get; set; }
        public string EmailID { get; set; }
    }

    public class TopAttendance
    {
        public int? AttendanceID { get; set; }
        public string Status { get; set; }
        public int? TeacherID { get; set; }
        public string TeacherName { get; set; }
        public int? ClassID { get; set; }
        public string Date { get; set; }
        public int? StudentID { get; set; }
        public string StudentName { get; set; }
    }

    public class TopGrade
    {
        public int? GradeID { get; set; }
        public int Score { get; set; }
        public int? TeacherID { get; set; }
        public int? StudentID { get; set; }
        public string StudentName { get; set; }
        public int? ClassID { get; set; }
        public int? SubjectID { get; set; }
        public string SubjectName { get; set; }
    }
    public class TeacherDashBoardModel
    {
        public List<DashboardCounts> Counts { get; set; }
        public List<RecentStudent> RecentStudents { get; set; }
        public List<RecentTeacher> RecentTeachers { get; set; }
        public List<TopAttendance> TopAttendances { get; set; }
        public List<TopGrade> TopGrades { get; set; }
    }
}
