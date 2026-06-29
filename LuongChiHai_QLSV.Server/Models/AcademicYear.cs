using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    public class AcademicYear
    {
        [Key]
        public int YearID { get; set; }
        public string YearName { get; set; } = string.Empty;
    }
}
