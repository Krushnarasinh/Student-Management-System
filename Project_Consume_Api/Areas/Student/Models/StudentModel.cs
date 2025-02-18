using System.ComponentModel.DataAnnotations;

namespace Project_Consume_Api.Areas.Student.Models
{
	public class StudentModel
	{
        public int? StudentID { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 100 characters.")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Phone number must be between 10 and 15 digits.")]
        public string ContactInfo { get; set; }

        [Required(ErrorMessage = "Roll Number is required.")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Roll Number must be between 3 and 10 characters.")]
        public string RollNo { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Class ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Class ID must be a positive number.")]
        public int ClassID { get; set; }

        public string? ClassName { get; set; }
    }

    public class StudentLogInModel
    {
        public StudentModel student { get; set; }
        public string token { get; set; }
    }
}