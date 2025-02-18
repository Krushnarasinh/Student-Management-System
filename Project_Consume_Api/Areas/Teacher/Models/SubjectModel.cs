using System.ComponentModel.DataAnnotations;

namespace Project_Consume_Api.Areas.Teacher.Models
{
    public class SubjectModel
    {
        public int SubjectID { get; set; }
        [Required(ErrorMessage = "Full Name is required.")]
        public string SubjectName { get; set; }
    }
    public class subjectdropdownmodel
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
    }
}
