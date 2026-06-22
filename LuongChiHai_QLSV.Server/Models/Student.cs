using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    [Table("Student")]
    public class Student
    {
        public string studentID { get; set; } = string.Empty;
        public string studentName { get; set; } = string.Empty;
        public DateTime? birthDate { get; set; }
        public string? gender { get; set; } = string.Empty;
        public string? email { get; set; } = string.Empty;

        // Navigation property (Mối quan hệ 1 - Nhiều với Enrollment)
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
