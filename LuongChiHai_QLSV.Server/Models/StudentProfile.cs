using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    [Table("StudentProfile")]
    public class StudentProfile
    {
        [Key]
        [ForeignKey("Student")]
        [Column("StudentID", TypeName = "varchar(15)")]
        public string StudentID { get; set; } = string.Empty;

        // -- Thông tin cơ bản --
        [Required]
        [Column("BirthDate")]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(30)]
        [Column("Ethnicity")]
        public string Ethnicity { get; set; } = "Kinh"; // Khớp DEFAULT N'Kinh'

        [Required]
        [StringLength(50)]
        [Column("Religion")]
        public string Religion { get; set; } = "Không"; // Khớp DEFAULT N'Không'

        [Required]
        [StringLength(50)]
        [Column("Nationality")]
        public string Nationality { get; set; } = "Việt Nam"; // Khớp DEFAULT N'Việt Nam'

        [Required]
        [StringLength(100)]
        [Column("BirthPlace")]
        public string BirthPlace { get; set; } = string.Empty;

        [Required]
        [StringLength(12)] // Khớp CHAR(12)
        [Column("CitizenID")]
        public string CitizenID { get; set; } = string.Empty;

        [Required]
        [Column("CitizenIDIssueDate")]
        public DateTime CitizenIDIssueDate { get; set; }

        [Required]
        [StringLength(100)]
        [Column("CitizenIDIssuePlace")]
        public string CitizenIDIssuePlace { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress] // Kiểm tra định dạng Email hợp lệ ở mức ứng dụng
        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("PermanentAddress")]
        public string PermanentAddress { get; set; } = "Chưa có"; // Khớp DEFAULT N'Chưa có'

        [Required]
        [StringLength(255)]
        [Column("TemporaryAddress")]
        public string TemporaryAddress { get; set; } = "Chưa có"; // Khớp DEFAULT N'Chưa có'

        // Thuộc tính điều hướng liên kết ngược về đối tượng Student chính
        public virtual Student? Student { get; set; }
    }
}
