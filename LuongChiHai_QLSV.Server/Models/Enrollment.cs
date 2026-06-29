using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public string StudentID { get; set; } = null!;
        public int CourseID { get; set; }
        public int AcademicYear { get; set; }
        public int Semester { get; set; }

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
