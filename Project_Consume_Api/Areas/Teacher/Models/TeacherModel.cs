using System.ComponentModel.DataAnnotations;

namespace Project_Consume_Api.Areas.Teacher.Models
{
    public class TeacherModel
    {
        public int? TeacherID { get; set; }

        [Required(ErrorMessage = "Teacher Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Teacher Name must be between 3 and 50 characters.")]
        public string TeacherName { get; set; }

        [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Contact Info must be a valid phone number.")]
        public string ContactInfo { get; set; }

        [Required(ErrorMessage = "Email ID is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Class selection is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid class.")]
        public int ClassID { get; set; }

        public string? ClassName { get; set; }
    }
    public class teacherdropdownmodel
    {
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
    }

    public class TeacherLoginResponse
    {
        public TeacherModel Teacher { get; set; }
        public string Token { get; set; }
    }
}
