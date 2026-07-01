using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Entities
{
    public class StudentStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusID { get; set; } // Primary Key

        [StringLength(50)]
        public string StatusName { get; set; } = string.Empty;

        public ICollection<AcademicProfile> AcademicProfiles { get; set; } = new List<AcademicProfile>();
    }
}
