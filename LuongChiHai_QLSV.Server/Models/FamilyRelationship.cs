using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    public class FamilyRelationship
    {
        public int FamilyMemberID { get; set; }
        public string StudentID { get; set; } = string.Empty;
        public string RelativeName { get; set; } = string.Empty;
        public string RelationshipType { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int BirthYear { get; set; }
        public virtual Student? Student { get; set; }
    }
}
