using LuongChiHai_QLSV.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuongChiHai_QLSV.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseOfferingsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public CourseOfferingsController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet("years")]
        public async Task<IActionResult> GetYears() => Ok(await _context.AcademicYears.ToListAsync());

        // Lấy danh sách học kỳ
        [HttpGet("semesters")]
        public async Task<IActionResult> GetSemesters() => Ok(await _context.SemesterTypes.ToListAsync());

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableCourses(string studentId, int yearId, int typeId)
        {
            var data = await _context.CourseOfferings
                .Include(co => co.Course)
                .Where(co =>
                    co.YearID == yearId &&
                    co.TypeID == typeId &&
                    co.IsOpen == true &&
                    !_context.Enrollments.Any(e =>
                        e.StudentID == studentId &&
                        e.OfferingID == co.OfferingID))
                .Select(co => new
                {
                    co.OfferingID,
                    co.Course.CourseCode,
                    co.Course.CourseName,
                    co.MaxStudents,
                    co.CurrentStudents
                })
                .ToListAsync();

            return Ok(data);
        }
    }
}
