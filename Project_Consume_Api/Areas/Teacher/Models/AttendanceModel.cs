namespace Project_Consume_Api.Areas.Teacher.Models
{
	public class AttendanceModel
	{
		public int AttendanceID { get; set; }
		public string Status { get; set; }
		public int TeacherID { get; set; }
		public int StudentID { get; set; }
		public string Date { get; set; }
	}
}
