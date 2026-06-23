using LuongChiHai_QLSV.Server.Models;
using LuongChiHai_QLSV.Server.Models.StudentDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly SchoolContext _context;
    public StudentsController(SchoolContext context)
    {
        _context = context;
    }

    // GET: api/Student
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
    {
        // Gọi trực tiếp DbSet để lấy toàn bộ bảng dữ liệu thô về bộ nhớ
        List<Student> allStudents = await _context.Students.ToListAsync();
        List<AcademicProfile> allAcademicProfiles = await _context.AcademicProfiles.ToListAsync();
        List<StudentProfile> allStudentProfiles = await _context.StudentProfiles.ToListAsync();

        // Khởi tạo danh sách kết quả trả về cho Angular
        List<StudentListDTO> resultList = new List<StudentListDTO>();

        foreach (var student in allStudents)
        {
            StudentListDTO dto = new StudentListDTO();
            dto.StudentID = student.StudentID;
            dto.StudentName = student.StudentName;
            dto.Gender = student.Gender;

            // Tìm thông tin học vấn từ bảng AcademicProfile dựa trên StudentID
            foreach (var academic in allAcademicProfiles)
            {
                if (academic.StudentID == student.StudentID)
                {
                    dto.ClassName = academic.ClassName;
                    dto.MajorName = academic.MajorName;
                    dto.Status = academic.Status;
                    break;
                }
            }

            // Tìm thông tin sinh viên từ bảng StudentProfile dựa trên StudentID
            foreach (var profile in allStudentProfiles)
            {
                if (profile.StudentID == student.StudentID)
                {
                    dto.PhoneNumber = profile.PhoneNumber;
                    break;
                }
            }

            // Sau khi gom đủ thông tin từ 3 bảng, thêm đối tượng DTO vào danh sách kết quả
            resultList.Add(dto);
        }
        return Ok(resultList);
    }

    // GET: api/Student/5
    [HttpGet("{studentid}")]
    public async Task<ActionResult<Student>> GetStudent(string studentid)
    {
        var student = await _context.Students.FindAsync(studentid);

        if (student == null)
        {
            return NotFound("Không tìm thấy sinh viên");
        }

        // Tiếp tục tìm bản ghi tương ứng ở các bảng phụ (vì Khóa chính của chúng cũng là StudentID)
        AcademicProfile? academic = await _context.AcademicProfiles.FindAsync(studentid);
        StudentProfile? profile = await _context.StudentProfiles.FindAsync(studentid);

        // Riêng bảng FamilyRelationship, khóa chính là FamilyMemberID (số tự tăng) chứ không phải StudentID.
        // Vì vậy ta phải load bảng này về và dùng vòng lặp để nhặt ra người thân của sinh viên này.
        List<FamilyRelationship> allFamilies = await _context.FamilyRelationships.ToListAsync();

        // Khởi tạo DTO chi tiết để đẩy lên Form Angular
        StudentDetailDTO detailDto = new StudentDetailDTO();

        // 1. Gán thông tin từ bảng Student
        detailDto.StudentID = student.StudentID;
        detailDto.StudentName = student.StudentName;
        detailDto.Gender = student.Gender;

        // 2. Gán thông tin từ bảng AcademicProfile (nếu có)
        if (academic != null)
        {
            detailDto.FileCode = academic.FileCode;
            detailDto.AdmissionDate = academic.AdmissionDate;
            detailDto.ClassName = academic.ClassName;
            detailDto.CampusName = academic.CampusName;
            detailDto.EducationLevel = academic.EducationLevel;
            detailDto.EducationType = academic.EducationType;
            detailDto.FacultyName = academic.FacultyName;
            detailDto.MajorName = academic.MajorName;
            detailDto.SpecializationName = academic.SpecializationName;
            detailDto.AcademicYear = academic.AcademicYear;
            detailDto.Status = academic.Status;
        }

        // 3. Gán thông tin từ bảng StudentProfile (nếu có)
        if (profile != null)
        {
            detailDto.BirthDate = profile.BirthDate;
            detailDto.Ethnicity = profile.Ethnicity;
            detailDto.Religion = profile.Religion;
            detailDto.Nationality = profile.Nationality;
            detailDto.BirthPlace = profile.BirthPlace;
            detailDto.CitizenID = profile.CitizenID;
            detailDto.CitizenIDIssueDate = profile.CitizenIDIssueDate;
            detailDto.CitizenIDIssuePlace = profile.CitizenIDIssuePlace;
            detailDto.PhoneNumber = profile.PhoneNumber;
            detailDto.Email = profile.Email;
            detailDto.PermanentAddress = profile.PermanentAddress;
            detailDto.TemporaryAddress = profile.TemporaryAddress;
        }

        // 4. Lọc danh sách người thân bằng vòng lặp foreach thuần túy
        detailDto.FamilyRelationships = new List<FamilyMemberDTO>();
        foreach (var member in allFamilies)
        {
            if (member.StudentID == studentid)
            {
                FamilyMemberDTO familyDto = new FamilyMemberDTO();
                familyDto.FamilyMemberID = member.FamilyMemberID;
                familyDto.RelativeName = member.RelativeName;
                familyDto.RelationshipType = member.RelationshipType;
                familyDto.PhoneNumber = member.PhoneNumber;
                familyDto.BirthYear = member.BirthYear;

                detailDto.FamilyRelationships.Add(familyDto);
            }
        }

        return Ok(detailDto);
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
    public async Task<ActionResult<Student>> PostStudent(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetStudent", new { studentid = student.StudentID }, student);
    }

    // DELETE: api/Student/5
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
