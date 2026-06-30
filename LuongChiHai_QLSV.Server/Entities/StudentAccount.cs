using LuongChiHai_QLSV.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Entities
{
    [Table("StudentAccount")]
    public class StudentAccount
    {
        [Key]
        [StringLength(15)]
        public string StudentID { get; set; } = string.Empty;

        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; } = null!;

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; } = null!;
    }
}
