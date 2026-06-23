using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    [Table("Student")]
    public class Student
    {
        [Key]
        [Column("StudentID", TypeName = "varchar(15)")]
        public string StudentID { get; set; } = string.Empty;

        [Required]
        [Column("StudentName")]
        [StringLength(100)]
        public string StudentName { get; set; } = string.Empty;

        [Column("Gender")]
        [StringLength(10)]
        public string? Gender { get; set; }

        // Mối quan hệ 1 - 1
        public virtual AcademicProfile? AcademicProfile { get; set; }
        public virtual StudentProfile? StudentProfile { get; set; }

        // Mối quan hệ 1 - Nhiều
        public virtual ICollection<FamilyRelationship> FamilyRelationships { get; set; } = new List<FamilyRelationship>();
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
