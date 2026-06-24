using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    [Table("Enrollment")]
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public string StudentID{ get; set; } = string.Empty;
        public int CourseID { get; set; }
        public DateTime EnrollmentDate { get; set; }


        // Liên kết nhiều-đến-1 về bảng Student và Course
        public virtual Student Student { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;

        // Liên kết 1-nhiều sang bảng Score (Một lượt đăng ký có nhiều đầu điểm)
        public virtual ICollection<Score> Scores { get; set; } = new List<Score>();
    }
}
