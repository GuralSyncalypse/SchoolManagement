using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    public class Score
    {
        public int ScoreId { get; set; }

        // Hai cột này tạo thành Foreign Key kép liên kết tới Enrollment
        public string StudentID { get; set; } = string.Empty;
        public int CourseID { get; set; }

        public string ScoreType { get; set; } = string.Empty;
        public decimal ScoreValue { get; set; }

        // Navigation property: Liên kết tới Enrollment
        // EF Core sẽ tự hiểu liên kết này dựa trên cấu hình Fluent API đã làm ở bước trước
        public virtual Enrollment Enrollment { get; set; } = null!;
    }
}
