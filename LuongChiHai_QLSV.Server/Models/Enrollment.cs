using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    public class Enrollment
    {
        public string StudentID { get; set; } = null!;
        public int CourseID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Semester { get; set; } = null!;

        // Điểm thành phần
        public decimal? ProcessScore { get; set; }
        public decimal? MidtermScore { get; set; }
        public decimal? FinalExamScore { get; set; }

        // Tỷ lệ phần trăm điểm
        public decimal WeightProcess { get; set; }
        public decimal WeightMidterm { get; set; }
        public decimal WeightFinal { get; set; }

        // Điểm tổng kết và kết quả
        public decimal? TotalScore { get; set; }
        public bool? IsPassed { get; set; }

        // Navigation Properties
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
