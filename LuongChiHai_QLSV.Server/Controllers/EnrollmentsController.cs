using Azure;
using Azure.Core;
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
            List<EnrollmentResponseDto> enrollments = await _context.Enrollments
                .Select(e => new EnrollmentResponseDto(
                    e.EnrollmentID,
                    e.StudentID,
                    e.OfferingID,
                    e.ProcessScore,
                    e.MidtermScore,
                    e.FinalExamScore,
                    e.TotalScore,
                    e.IsPassed
                ))
                .ToListAsync();

            // Trả về danh sách đã được định kiểu rõ ràng
            return Ok(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentRequestDto dto)
        {
            var errors = new Dictionary<string, string[]>();

            // 1. Kiểm tra Trùng khóa (Duplicate Key)
            var isDuplicate = await _context.Enrollments
                .AnyAsync(e => e.StudentID == dto.StudentID && e.OfferingID == dto.OfferingID);

            if (isDuplicate)
            {
                errors.Add("Conflict", new[] { $"Sinh viên {dto.StudentID} đã đăng ký với mã môn học {dto.OfferingID} trong học kỳ rồi." });
            }

            // 2. Kiểm tra Môn học
            var courseExists = await _context.CourseOfferings.AnyAsync(c => c.OfferingID == dto.OfferingID);
            if (!courseExists)
            {
                errors.Add("OfferingID", new[] { $"Môn học với ID {dto.OfferingID} không tồn tại." });
            }

            // 3. Kiểm tra Sinh viên
            var studentExists = await _context.Students.AnyAsync(s => s.StudentID == dto.StudentID);
            if (!studentExists)
            {
                errors.Add("StudentID", new[] { $"Sinh viên với ID {dto.StudentID} không tồn tại." });
            }


            // 4. HIỆU CHỈNH TẠI ĐÂY: Gom lỗi và đính kèm Trace ID của request
            if (errors.Any())
            {
                var problemDetails = new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "One or more validation errors occurred.",
                    // Lấy mã TraceIdentifier tự động từ request hiện tại của hệ thống
                    Extensions = { ["traceId"] = HttpContext.TraceIdentifier }
                };

                return BadRequest(problemDetails);
            }

            // 5. Lưu vào Database nếu mọi thứ hợp lệ
            var enrollment = new Enrollment
            {
                StudentID = dto.StudentID,
                OfferingID = dto.OfferingID,
                ProcessScore = dto.ProcessScore,
                MidtermScore = dto.MidtermScore,
                FinalExamScore = dto.FinalExamScore
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentResponseDto>> GetEnrollment(int id)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.EnrollmentID == id);

            if (enrollment == null) return NotFound();

            EnrollmentResponseDto dto = new EnrollmentResponseDto(
                enrollment.EnrollmentID,
                enrollment.StudentID,
                enrollment.OfferingID,
                enrollment.ProcessScore,
                enrollment.MidtermScore,
                enrollment.FinalExamScore,
                enrollment.TotalScore,
                enrollment.IsPassed
            );


            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnrollment(int id, [FromBody] EnrollmentRequestDto dto)
        {
            // 1. Tìm bản ghi dựa trên khóa chính phức hợp
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi để cập nhật." });
            }

            // 2. Cập nhật các thuộc tính
            enrollment.OfferingID = dto.OfferingID;
            enrollment.ProcessScore = dto.ProcessScore;
            enrollment.MidtermScore = dto.MidtermScore;
            enrollment.FinalExamScore = dto.FinalExamScore;
    

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
        [HttpDelete("{id}")] // URL: api/enrollments/STU001/101
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            // Tìm bản ghi theo khóa chính kép
            var enrollment = await _context.Enrollments.FindAsync(id);

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
