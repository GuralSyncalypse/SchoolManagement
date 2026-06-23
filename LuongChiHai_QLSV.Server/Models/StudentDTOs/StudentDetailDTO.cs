namespace LuongChiHai_QLSV.Server.Models.StudentDTOs
{
    public class StudentDetailDTO
    {
        // Thông tin cơ bản
        public string StudentID { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string? Gender { get; set; }

        // Thông tin học vấn (Hút từ AcademicProfile)
        public DateTime AdmissionDate { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string CampusName { get; set; } = string.Empty;
        public string EducationLevel { get; set; } = "Đại học";
        public string EducationType { get; set; } = "Chính quy";
        public string FacultyName { get; set; } = string.Empty;
        public string MajorName { get; set; } = string.Empty;
        public string? SpecializationName { get; set; }
        public int AcademicYear { get; set; }
        public string Status { get; set; } = string.Empty;

        // Thông tin cá nhân (Hút từ StudentProfile)
        public DateTime BirthDate { get; set; }
        public string Ethnicity { get; set; } = string.Empty;
        public string Religion { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string BirthPlace { get; set; } = string.Empty;
        public string CitizenID { get; set; } = string.Empty;
        public DateTime CitizenIDIssueDate { get; set; }
        public string CitizenIDIssuePlace { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PermanentAddress { get; set; } = string.Empty;
        public string TemporaryAddress { get; set; } = string.Empty;

        // Danh sách người thân (Dùng kèm một DTO nhỏ cho Family)
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
