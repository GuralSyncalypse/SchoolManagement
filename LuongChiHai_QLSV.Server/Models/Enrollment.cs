using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    public class Enrollment
    {
        public string StudentID { get; set; } = string.Empty;
        public int CourseID { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        // Navigation properties (tùy chọn, dùng cho EF Core)
        public Student? Student { get; set; }
        public Course? Course { get; set; }

        // Liên kết 1-nhiều sang bảng Score (Một lượt đăng ký có nhiều đầu điểm)
        public virtual ICollection<Score> Scores { get; set; } = new List<Score>();
    }
}
