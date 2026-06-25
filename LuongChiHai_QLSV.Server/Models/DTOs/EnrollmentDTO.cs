namespace LuongChiHai_QLSV.Server.Models.DTOs
{
    // DTO dùng khi tạo mới hoặc cập nhật điểm
    public record EnrollmentRequestDto(
        string StudentID,
        int CourseID,
        string Semester,
        decimal? ProcessScore,
        decimal? MidtermScore,
        decimal? FinalExamScore
    );

    // DTO dùng khi trả dữ liệu về cho Frontend
    public record EnrollmentResponseDto(
        string StudentID,
        string CourseID,
        string Semester,
        decimal? ProcessScore,
        decimal? MidtermScore,
        decimal? FinalExamScore,
        decimal? TotalScore,
        bool? IsPassed
    );
}
