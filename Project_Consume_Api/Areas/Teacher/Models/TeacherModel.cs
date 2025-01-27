namespace Project_Consume_Api.Areas.Teacher.Models
{
    public class TeacherModel
    {
        public int? TeacherID { get; set; }
        public string TeacherName { get; set; }
        public string ContactInfo { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public int ClassID { get; set; }
    }
}
