using System.ComponentModel.DataAnnotations;

namespace LuongChiHai_QLSV.Server.Models.DTOs
{
    // DTO dùng khi tạo mới hoặc cập nhật điểm
    public record EnrollmentRequestDto(
        [Required(ErrorMessage = "Mã sinh viên không được để trống")]
        string StudentID,

        [Required(ErrorMessage = "Mã môn học không được để trống")]
        int CourseID,

        [Required(ErrorMessage = "Học kỳ không được để trống")]
        string Semester,

        // Chặn khoảng từ 0.0 đến 10.0 cho các trường điểm
        [Range(0.0, 10.0, ErrorMessage = "Điểm quá trình phải nằm trong khoảng từ 0.0 đến 10.0")]
        decimal? ProcessScore,

        [Range(0.0, 10.0, ErrorMessage = "Điểm giữa kỳ phải nằm trong khoảng từ 0.0 đến 10.0")]
        decimal? MidtermScore,

        [Range(0.0, 10.0, ErrorMessage = "Điểm cuối kỳ phải nằm trong khoảng từ 0.0 đến 10.0")]
        decimal? FinalExamScore
    );

    // DTO dùng khi trả dữ liệu về cho Frontend
    public record EnrollmentResponseDto(
        string StudentID,
        int CourseID,
        string Semester,
        decimal? ProcessScore,
        decimal? MidtermScore,
        decimal? FinalExamScore,
        decimal? TotalScore,
        bool? IsPassed
    );
}
