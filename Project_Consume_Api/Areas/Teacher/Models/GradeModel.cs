using System.ComponentModel.DataAnnotations;

namespace Project_Consume_Api.Areas.Teacher.Models
{
	public class GradeModel
	{
        public int GradeID { get; set; }
        [Required]
    [Range(0, 100, ErrorMessage = "Score must be between 0 and 100.")]
        public int Score { get; set; }
        public int TeacherID { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public string? TeacherName { get; set; }
        public string? StudentName { get; set; }
        public string? SubjectName { get; set; }

        public int ClassID { get; set; }

        public string? RollNo { get; set; }
    }

    public class GradeClassListModel
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public Boolean IsFieldGrade { get; set; }
    }
    public class ForGradeTeacherDropDownModel
	{
		public int TeacherID { get; set; }

		public string TeacherName { get; set; }
	}

	public class ForGradeStudentDropDownModel
	{
		public int StudentID { get; set; }

		public string StudentName { get; set; }
	}

	public class ForGradeSubjectDropDownModel
	{
		public int SubjectID { get; set; }

		public string SubjectName { get; set; }
	}
}
