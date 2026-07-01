namespace LuongChiHai_QLSV.Server.DTOs.Students
{
    public class StudentResponseDto
    {
        public string StudentID { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public string? Ethnicity { get; set; }
        public string? PermanentAddress { get; set; }

        // Thông tin liên quan
        public AcademicProfileDto? AcademicProfile { get; set; }

        // Khởi tạo List để tránh null khi truy cập
        public List<FamilyRelationshipDto> FamilyRelationships { get; set; } = new List<FamilyRelationshipDto>();
    }

    public class AcademicProfileDto
    {
        public string? ClassName { get; set; } = string.Empty;
        public string? FacultyName { get; set; } = string.Empty;
        public string? MajorName { get; set; } = string.Empty;
    }

    public class FamilyRelationshipDto
    {
        public string? RelativeName { get; set; } = string.Empty;
        public string? RelationshipType { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
