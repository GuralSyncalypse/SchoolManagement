using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    public class Student
    {
        public string StudentID { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        // Mối quan hệ 1 - 1
        public virtual AcademicProfile? AcademicProfile { get; set; }
        public virtual StudentProfile? StudentProfile { get; set; }

        // Mối quan hệ 1 - Nhiều
        public virtual ICollection<FamilyRelationship> FamilyRelationships { get; set; } = new List<FamilyRelationship>();
    }
}
