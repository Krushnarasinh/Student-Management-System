namespace Project_Consume_Api.Areas.Student.Models
{
	public class StudentModel
	{
		public int? StudentID { get; set; }
		public string StudentName { get; set; }
		public DateTime DOB { get; set; }
		public string Gender { get; set; }
		public string ContactInfo { get; set; }
		public string RollNo { get; set; }
		public string EmailID { get; set; }
		public string Password { get; set; }
		public int ClassID { get; set; }
		public string? ClassName { get; set; }
	}
}