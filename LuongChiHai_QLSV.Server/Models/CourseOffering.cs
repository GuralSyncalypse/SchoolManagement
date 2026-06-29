using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuongChiHai_QLSV.Server.Models
{
    public class CourseOffering
    {
        public int OfferingID { get; set; }
        public int CourseID { get; set; }
        public int YearID { get; set; }
        public int TypeID { get; set; }
        public int? InstructorID { get; set; }
        public int? MaxStudents { get; set; }
        public int CurrentStudents { get; set; } = 0;
        public bool? IsOpen { get; set; }

        // Navigation Properties
        public required Course Course { get; set; }
        public required AcademicYear AcademicYear { get; set; }
        public required SemesterType SemesterType { get; set; }
    }
}
