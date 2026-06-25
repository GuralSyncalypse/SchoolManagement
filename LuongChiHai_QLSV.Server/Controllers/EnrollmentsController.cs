using LuongChiHai_QLSV.Server.Data;
using LuongChiHai_QLSV.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuongChiHai_QLSV.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public EnrollmentsController(SchoolContext context) => _context = context;

        // GET: api/Enrollments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments()
            => await _context.Enrollments.ToListAsync();

        // GET: api/Enrollments/SV001/101
        [HttpGet("{studentId}/{courseId}")]
        public async Task<ActionResult<Enrollment>> GetEnrollment(string studentId, int courseId)
        {
            var enrollment = await _context.Enrollments.FindAsync(studentId, courseId);
            return enrollment == null ? NotFound() : enrollment;
        }

        // PUT: api/Enrollments/SV001/101
        [HttpPut("{studentId}/{courseId}")]
        public async Task<IActionResult> PutEnrollment(string studentId, int courseId, Enrollment enrollment)
        {
            // Kiểm tra xem ID trên URL có khớp với dữ liệu trong body không
            if (studentId != enrollment.StudentID || courseId != enrollment.CourseID)
            {
                return BadRequest("ID trong URL không khớp với dữ liệu gửi lên.");
            }

            // Đánh dấu thực thể là đã sửa đổi (Modified)
            _context.Entry(enrollment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(studentId, courseId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Hàm hỗ trợ kiểm tra tồn tại
        private bool EnrollmentExists(string studentId, int courseId)
        {
            return _context.Enrollments.Any(e => e.StudentID == studentId && e.CourseID == courseId);
        }

        // POST: api/Enrollments
        [HttpPost]
        public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEnrollment),
                new { studentId = enrollment.StudentID, courseId = enrollment.CourseID }, enrollment);
        }

        // DELETE: api/Enrollments/SV001/101
        [HttpDelete("{studentId}/{courseId}")]
        public async Task<IActionResult> DeleteEnrollment(string studentId, int courseId)
        {
            var enrollment = await _context.Enrollments.FindAsync(studentId, courseId);
            if (enrollment == null) return NotFound();

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
