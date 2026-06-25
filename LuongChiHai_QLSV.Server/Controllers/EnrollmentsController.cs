using Azure;
using LuongChiHai_QLSV.Server.Data;
using LuongChiHai_QLSV.Server.Models;
using LuongChiHai_QLSV.Server.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuongChiHai_QLSV.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public EnrollmentsController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDto>>> GetEnrollments()
        {
            var enrollments = await _context.Enrollments
                .Select(e => new EnrollmentResponseDto(
                    e.StudentID,
                    e.CourseID.ToString(), // Chuyển đổi nếu kiểu dữ liệu khác nhau
                    e.Semester,
                    e.ProcessScore,
                    e.MidtermScore,
                    e.FinalExamScore,
                    e.TotalScore,
                    e.IsPassed
                ))
                .ToListAsync();

            return Ok(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentRequestDto dto)
        {
            var enrollment = new Enrollment
            {
                StudentID = dto.StudentID,
                CourseID = dto.CourseID,
                Semester = dto.Semester,
                ProcessScore = dto.ProcessScore,
                MidtermScore = dto.MidtermScore,
                FinalExamScore = dto.FinalExamScore
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok(enrollment);
        }

        [HttpGet("{studentId}/{courseId}")]
        public async Task<ActionResult<EnrollmentResponseDto>> GetEnrollment(string studentId, int courseId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentID == studentId && e.CourseID == courseId);

            if (enrollment == null) return NotFound();

            EnrollmentResponseDto dto = new EnrollmentResponseDto(
                enrollment.StudentID,
                enrollment.CourseID.ToString(),
                enrollment.Semester,
                enrollment.ProcessScore,
                enrollment.MidtermScore,
                enrollment.FinalExamScore,
                enrollment.TotalScore,
                enrollment.IsPassed
            );


            return Ok(dto);
        }

        [HttpPut("{studentId}/{courseId}")]
        public async Task<IActionResult> UpdateEnrollment(string studentId, int courseId, [FromBody] EnrollmentRequestDto dto)
        {
            // 1. Tìm bản ghi dựa trên khóa chính phức hợp
            var enrollment = await _context.Enrollments.FindAsync(studentId, courseId);

            if (enrollment == null)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi để cập nhật." });
            }

            // 2. Cập nhật các thuộc tính
            enrollment.Semester = dto.Semester;
            enrollment.ProcessScore = dto.ProcessScore;
            enrollment.MidtermScore = dto.MidtermScore;
            enrollment.FinalExamScore = dto.FinalExamScore;
            // Không cập nhật StudentID và CourseID vì đây là khóa chính

            // 3. Lưu thay đổi
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Có lỗi xảy ra khi cập nhật dữ liệu.");
            }

            return NoContent(); // Trả về 204 No Content sau khi update thành công
        }

        // DELETE: api/Student/5
        [HttpDelete("{studentId}/{courseId}")] // URL: api/enrollments/STU001/101
        public async Task<IActionResult> DeleteEnrollment(string studentId, int courseId)
        {
            // Tìm bản ghi theo khóa chính kép
            var enrollment = await _context.Enrollments.FindAsync(studentId, courseId);

            if (enrollment == null)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi cần xóa." });
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
