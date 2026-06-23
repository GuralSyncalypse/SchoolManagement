using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    [Table("FamilyRelationship")]
    public class FamilyRelationship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FamilyMemberID { get; set; }

        [Required]
        [Column("StudentID", TypeName = "varchar(15)")]
        public string StudentID { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string RelativeName { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string RelationshipType { get; set; } = string.Empty;

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        public int BirthYear { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student? Student { get; set; }
    }
}
