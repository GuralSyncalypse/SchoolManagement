using LuongChiHai_QLSV.Server.Data;
using LuongChiHai_QLSV.Server.DTOs.Auths;
using LuongChiHai_QLSV.Server.DTOs.Students;
using LuongChiHai_QLSV.Server.Entities;
using LuongChiHai_QLSV.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminStudentsController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly IStudentService _studentService;
    public AdminStudentsController(SchoolContext context, IStudentService studentService)
    {
        _context = context;
        _studentService = studentService;
    }

    // GET: api/Student
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetStudent()
    {
        var students = await _context.Students
            .Include(s => s.AcademicProfile)
            .Include(s => s.FamilyRelationships)
            .ToListAsync();

        if (students == null || !students.Any()) return NotFound("Không tìm thấy sinh viên nào.");
            
        // 2. Chuyển đổi (Map) thủ công từ Entity sang DTO
        var response = students.Select(s => new StudentResponseDto
        {
            StudentID = s.StudentID,
            StudentName = s.StudentName,
            Gender = s.Gender,
            Ethnicity = s.Ethnicity,
            PermanentAddress = s.PermanentAddress,

            // Map object lồng nhau (AcademicProfile)
            AcademicProfile = s.AcademicProfile == null ? null : new AcademicProfileDto
            {
                ClassName = s.AcademicProfile.ClassName,
                FacultyName = s.AcademicProfile.FacultyName,
                MajorName = s.AcademicProfile.MajorName
            },

            // Map collection lồng nhau (FamilyRelationships)
            FamilyRelationships = s.FamilyRelationships?.Select(fr => new FamilyRelationshipDto
            {
                RelativeName = fr.RelativeName,
                RelationshipType = fr.RelationshipType,
                PhoneNumber = fr.PhoneNumber ?? ""
            }).ToList() ?? new List<FamilyRelationshipDto>()
        }).ToList();

        // Map Entity sang DTO
        return Ok(response);
    }

    // GET: api/Student/5
    [HttpGet("{studentid}")]
    public async Task<ActionResult<StudentResponseDto>> GetStudent(string studentid)
    {
        // 1. Sử dụng Include để lấy các thông tin liên quan
        var student = await _context.Students
            .Include(s => s.AcademicProfile)
            .Include(s => s.FamilyRelationships)
            .FirstOrDefaultAsync(s => s.StudentID == studentid);

        if (student == null)
        {
            return NotFound($"Không tìm thấy sinh viên có mã: {studentid}");
        }

        // 2. Map thủ công sang DTO
        var response = new StudentResponseDto
        {
            StudentID = student.StudentID,
            StudentName = student.StudentName,
            Gender = student.Gender,
            Ethnicity = student.Ethnicity,
            PermanentAddress = student.PermanentAddress,

            AcademicProfile = student.AcademicProfile == null ? null : new AcademicProfileDto
            {
                ClassName = student.AcademicProfile.ClassName,
                FacultyName = student.AcademicProfile.FacultyName,
                MajorName = student.AcademicProfile.MajorName
            },

            FamilyRelationships = student.FamilyRelationships?.Select(fr => new FamilyRelationshipDto
            {
                RelativeName = fr.RelativeName,
                RelationshipType = fr.RelationshipType,
                PhoneNumber = fr.PhoneNumber ?? ""
            }).ToList() ?? new List<FamilyRelationshipDto>()
        };

        return Ok(response);
    }

    // PUT: api/Student/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{studentid}")]
    public async Task<IActionResult> PutStudent(string? studentid, Student student)
    {
        if (studentid != student.StudentID)
        {
            return BadRequest();
        }

        _context.Entry(student).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExists(studentid))
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

    // POST: api/Student
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<StudentResponseDto>> PostStudent(StudentRequestDto request)
    {
        try
        {
            await _studentService.CreateStudentAccountAsync(request);

            var response = new StudentResponseDto
            {
                StudentID = request.StudentID,
                StudentName = request.StudentName,
                Gender = request.Gender,
                Ethnicity = request.Ethnicity,
                PermanentAddress = request.PermanentAddress
            };

            return CreatedAtAction(nameof(GetStudent), new { studentid = request.StudentID }, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Có lỗi xảy ra",
                error = ex.Message
            });
        }
    }

    // DELETE: api/Student/5
    [HttpDelete("{studentid}")]
    public async Task<IActionResult> DeleteStudent(string? studentid)
    {
        if (studentid == null)
            return BadRequest("Mã sinh viên không được để trống.");

        await _studentService.DeleteAsync(studentid);

        return NoContent();
    }

    private bool StudentExists(string? studentid)
    {
        return _context.Students.Any(e => e.StudentID == studentid);
    }
}
