using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Entities
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public int? Credits { get; set; } // Số tín chỉ

    }
}
