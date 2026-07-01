using LuongChiHai_QLSV.Server.Data;
using LuongChiHai_QLSV.Server.DTOs.Students;
using LuongChiHai_QLSV.Server.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly SchoolContext _context;
    public StudentsController(SchoolContext context)
    {
        _context = context;
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
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<StudentResponseDto>> PostStudent(StudentRequestDto request)
    {
        // 1. Kiểm tra validation thủ công (tuỳ chọn vì [ApiController] đã làm điều này)
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // 2. Kiểm tra trùng lặp mã sinh viên
        if (await _context.Students.AnyAsync(s => s.StudentID == request.StudentID))
        {
            return Conflict(new { message = $"Mã sinh viên {request.StudentID} đã tồn tại trong hệ thống." });
        }

        // 3. Map từ DTO sang Entity
        var newStudent = new Student
        {
            StudentID = request.StudentID,
            UserID = request.UserID,
            StudentName = request.StudentName,
            Gender = request.Gender,
            BirthDate = request.BirthDate,
            Ethnicity = request.Ethnicity,
            Religion = request.Religion,
            Nationality = request.Nationality,
            BirthPlace = request.BirthPlace,
            CitizenID = request.CitizenID,
            CitizenIDIssueDate = request.CitizenIDIssueDate,
            CitizenIDIssuePlace = request.CitizenIDIssuePlace,
            PermanentAddress = request.PermanentAddress,
            TemporaryAddress = request.TemporaryAddress
        };

        // 4. Lưu vào cơ sở dữ liệu
        _context.Students.Add(newStudent);
        await _context.SaveChangesAsync();

        // 5. Map sang ResponseDTO để trả về (Tránh lỗi Object Cycle)
        var response = new StudentResponseDto
        {
            StudentID = newStudent.StudentID,
            StudentName = newStudent.StudentName,
            Gender = newStudent.Gender,
            Ethnicity = newStudent.Ethnicity,
            PermanentAddress = newStudent.PermanentAddress
            // Có thể map thêm các field khác nếu cần
        };

        // 6. Trả về mã 201 Created cùng với location của resource vừa tạo
        return CreatedAtAction(nameof(GetStudent), new { studentid = newStudent.StudentID }, response);
    }

    // DELETE: api/Student/5
    [Authorize(Roles = "Admin")]
    [HttpDelete("{studentid}")]
    public async Task<IActionResult> DeleteStudent(string? studentid)
    {
        var student = await _context.Students.FindAsync(studentid);
        if (student == null)
        {
            return NotFound();
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StudentExists(string? studentid)
    {
        return _context.Students.Any(e => e.StudentID == studentid);
    }
}
