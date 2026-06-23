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

        [Required]
        [StringLength(30)]
        public string FileCode { get; set; } = string.Empty;

        public DateTime AdmissionDate { get; set; }

        [Required]
        [StringLength(50)]
        public string ClassName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CampusName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string EducationLevel { get; set; } = "Đại học";

        [Required]
        [StringLength(50)]
        public string EducationType { get; set; } = "Chính quy";

        [Required]
        [StringLength(100)]
        public string FacultyName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string MajorName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? SpecializationName { get; set; }

        public int AcademicYear { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Đang học";

        // Navigation property ngược lại Student
        public virtual Student? Student { get; set; }
    }
}
