namespace Project_Consume_Api.Areas.Teacher.Models
{
	public class AttendanceModel
	{
		public int AttendanceID { get; set; }
		public string Status { get; set; }
		public int TeacherID { get; set; }
		public int StudentID { get; set; }
		public int ClassID { get; set; }

		public string StudentName { get; set; }
		public string RollNo { get; set; }
		public string Date { get; set; }
	}
	public class AttendanceListModel
	{
		public int ClassID { get; set; }
		public string ClassName { get; set; }
		public Boolean IsFieldAttendance { get; set; }
	}
}
