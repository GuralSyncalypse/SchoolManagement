using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    [Table("Course")]
    public class Course
    {
        public int courseID { get; set; }
        public string courseName { get; set; } = string.Empty;
        public string courseCode { get; set; } = string.Empty;
        public int? credits { get; set; } // Số tín chỉ

        // Navigation property (Mối quan hệ 1 - Nhiều với Enrollment)
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
