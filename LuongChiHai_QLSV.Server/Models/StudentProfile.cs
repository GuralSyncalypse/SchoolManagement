using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    public class StudentProfile
    {
        // Bắt buộc
        public string StudentID { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; 

        // Các trường cho phép NULL (Bổ sung sau)
        public DateTime? BirthDate { get; set; }
        public string? Ethnicity { get; set; }
        public string? Religion { get; set; }
        public string? Nationality { get; set; }
        public string? BirthPlace { get; set; }
        public string? CitizenID { get; set; }
        public DateTime? CitizenIDIssueDate { get; set; }
        public string? CitizenIDIssuePlace { get; set; }
        public string? PermanentAddress { get; set; }
        public string? TemporaryAddress { get; set; }

        // Quan hệ điều hướng
        public virtual Student? Student { get; set; }
    }
}
