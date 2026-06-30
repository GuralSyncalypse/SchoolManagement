namespace LuongChiHai_QLSV.Server.DTOs.Students
{
    public class StudentDetailDTO
    {
        // Thông tin cơ bản
        public string StudentID { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        // Thông tin học vấn
        public DateTime? AdmissionDate { get; set; }
        public string? ClassName { get; set; }
        public string? CampusName { get; set; }
        public string? EducationLevel { get; set; }
        public string? EducationType { get; set; }
        public string? FacultyName { get; set; }
        public string? MajorName { get; set; }
        public string? SpecializationName { get; set; }
        public int? AcademicYear { get; set; }
        public string Status { get; set; } = "Đang học";

        // Thông tin cá nhân
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string? Ethnicity { get; set; }
        public string? Religion { get; set; }
        public string? Nationality { get; set; }
        public string? BirthPlace { get; set; }
        public string? CitizenID { get; set; }
        public DateTime? CitizenIDIssueDate { get; set; }
        public string? CitizenIDIssuePlace { get; set; }
        public string? PermanentAddress { get; set; }
        public string? TemporaryAddress { get; set; }

        // Danh sách người thân
        public List<FamilyMemberDTO> FamilyRelationships { get; set; } = new List<FamilyMemberDTO>();
    }

    public class FamilyMemberDTO
    {
        public int FamilyMemberID { get; set; }
        public string RelativeName { get; set; } = string.Empty;
        public string RelationshipType { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int BirthYear { get; set; }
    }
}
