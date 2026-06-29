using System.ComponentModel.DataAnnotations;

namespace LuongChiHai_QLSV.Server.Models.DTOs
{
    // DTO dùng khi tạo mới hoặc cập nhật điểm
    public record EnrollmentRequestDto(
        [Required(ErrorMessage = "Mã sinh viên không được để trống")]
        string StudentID,

        [Required(ErrorMessage = "Mã môn học không được để trống")]
        int CourseID,

        [Required(ErrorMessage = "Năm học không được để trống")]
        int AcademicYear,

        [Range(1, 3, ErrorMessage = "Học kỳ phải từ 1 đến 3")]
        int Semester,

        [Range(0.0, 10.0, ErrorMessage = "Điểm quá trình phải từ 0.0 đến 10.0")]
        decimal? ProcessScore,

        [Range(0.0, 10.0, ErrorMessage = "Điểm giữa kỳ phải từ 0.0 đến 10.0")]
        decimal? MidtermScore,

        [Range(0.0, 10.0, ErrorMessage = "Điểm cuối kỳ phải từ 0.0 đến 10.0")]
        decimal? FinalExamScore
    );

    // DTO dùng khi trả dữ liệu về cho Frontend
    public record EnrollmentResponseDto(
        int EnrollmentID,
        string StudentID,
        int CourseID,
        int AcademicYear,
        int Semester,
        decimal? ProcessScore,
        decimal? MidtermScore,
        decimal? FinalExamScore,
        decimal? TotalScore,
        bool? IsPassed
    ) {
        // Thuộc tính tính toán để hiển thị ra UI
        public string SemesterDisplay => $"HK{Semester}-{AcademicYear}-{AcademicYear + 1}";
    }
}
