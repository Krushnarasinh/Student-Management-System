namespace Project_Consume_Api.Areas.Student.Models
{
    public class StudentDashboardCounts
    {
        public string Metric { get; set; }
        public int Value { get; set; }
    }
    public class StudentInfo
    {
        public int? StudentID { get; set; }
        public string StudentName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string ContactInfo { get; set; }
        public string RollNo { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public int? ClassID { get; set; }
        public string ClassName { get; set; }
        public int? SubjectID { get; set; }
    }

    public class AttendanceInfo
    {
        public int? AttendanceID { get; set; }
        public string Status { get; set; }
        public int? StudentID { get; set; }
        public int? TeacherID { get; set; }
        public string Date { get; set; }
        public int? ClassID { get; set; }
    }

    public class GradeInfo
    {
        public int? GradeID { get; set; }
        public int Score { get; set; }
        public int? StudentID { get; set; }
        public int? TeacherID { get; set; }
        public int? SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int? ClassID { get; set; }
    }

    public class StudentDashBoardModel
    {
        public List<StudentDashboardCounts> Counts { get; set; }
        public List<StudentInfo> StudentDetails { get; set; }
        public List<AttendanceInfo> AttendanceRecords { get; set; }
        public List<GradeInfo> GradeRecords { get; set; }
    }
}
