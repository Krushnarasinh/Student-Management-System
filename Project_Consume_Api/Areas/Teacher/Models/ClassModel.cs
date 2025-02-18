using System.ComponentModel.DataAnnotations;
namespace Project_Consume_Api.Models
{
    public class ClassModel
    {
        public int ClassID { get; set; }
        [Required(ErrorMessage = "Full Name is required.")]
        public string ClassName { get; set; }
    }
	public class classdropdownmodel
	{
		public int ClassID { get; set; }
		public string ClassName { get; set; }
	}
}