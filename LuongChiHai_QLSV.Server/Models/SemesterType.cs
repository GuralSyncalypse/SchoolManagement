using System.ComponentModel.DataAnnotations;

namespace LuongChiHai_QLSV.Server.Models
{
    public class SemesterType
    {
        [Key]
        public int TypeID { get; set; }
        public string TypeName { get; set; } = string.Empty;
    }
}
