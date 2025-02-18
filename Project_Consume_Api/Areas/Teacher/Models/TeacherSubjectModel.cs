using System.ComponentModel.DataAnnotations;

namespace Project_Consume_Api.Areas.Teacher.Models
{
	public class TeacherSubjectModel
	{
		public int TeacherSubjectID { get; set; }
        
        [Required(ErrorMessage = "TeacherSubjectName is required.")]
        [StringLength(100, ErrorMessage = "TeacherSubjectName must not exceed 100 characters.")]
        public string TeacherSubjectName { get; set; }

        [Required(ErrorMessage = "SubjectID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "SubjectID must be a positive integer.")]
        public int SubjectID { get; set; }

        [Required(ErrorMessage = "TeacherID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TeacherID must be a positive integer.")]
        public int TeacherID { get; set; }
    }
}
