using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    [Table("Score")]
    public class Score
    {
        public int ScoreId { get; set; }
        public int EnrollmentId { get; set; } // Khóa ngoại kết nối với Enrollment
        public string ScoreType { get; set; } = string.Empty; // Ví dụ: "Giữa kỳ", "Cuối kỳ"
        public decimal ScoreValue { get; set; } // DECIMAL(4,2) tương ứng với decimal trong C#

        // Liên kết ngược về lượt đăng ký học của điểm số này
        public virtual Enrollment Enrollment { get; set; } = null!;
    }
}
