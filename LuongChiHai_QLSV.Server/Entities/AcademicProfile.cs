using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    [Table("AcademicProfile")]
    public class AcademicProfile
    {
        [Key]
        [ForeignKey("Student")]
        [Column("StudentID", TypeName = "varchar(15)")]
        public string StudentID { get; set; } = string.Empty;

        public DateTime? AdmissionDate { get; set; }

        [StringLength(50)]
        public string? ClassName { get; set; }

        [StringLength(100)]
        public string? CampusName { get; set; }

        [StringLength(50)]
        public string? EducationLevel { get; set; }
        [StringLength(50)]
        public string? EducationType { get; set; }

        [StringLength(100)]
        public string? FacultyName { get; set; }

        [StringLength(100)]
        public string? MajorName { get; set; }

        [StringLength(100)]
        public string? SpecializationName { get; set; }

        public int? AcademicYear { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Đang học";

        // Navigation property ngược lại Student
        public virtual Student? Student { get; set; }
    }
}
