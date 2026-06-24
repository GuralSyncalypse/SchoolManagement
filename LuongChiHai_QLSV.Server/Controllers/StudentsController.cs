using LuongChiHai_QLSV.Server.Data;
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
            detailDto.AdmissionDate = academic.AdmissionDate ?? null;
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
            detailDto.BirthDate = profile.BirthDate ?? null;
            detailDto.Ethnicity = profile.Ethnicity;
            detailDto.Religion = profile.Religion;
            detailDto.Nationality = profile.Nationality;
            detailDto.BirthPlace = profile.BirthPlace;
            detailDto.CitizenID = profile.CitizenID;
            detailDto.CitizenIDIssueDate = profile.CitizenIDIssueDate ?? null;
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
    public async Task<IActionResult> PutStudent(string studentid, [FromBody] StudentDetailDTO updateDto)
    {
        if (studentid != updateDto.StudentID) return BadRequest("Mã sinh viên không khớp.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Tìm các thực thể hiện tại trong DB
            var student = await _context.Students.FindAsync(studentid);
            var profile = await _context.StudentProfiles.FindAsync(studentid);
            var academic = await _context.AcademicProfiles.FindAsync(studentid);
            var family = await _context.FamilyRelationships
                .Where(f => f.StudentID == studentid)
                .ToListAsync();

            if (student == null || profile == null || academic == null)
                return NotFound("Không tìm thấy hồ sơ để cập nhật.");

            // 2. Cập nhật bảng Student
            student.StudentName = updateDto.StudentName;
            student.Gender = updateDto.Gender;

            // 3. Cập nhật bảng StudentProfile
            profile.PhoneNumber = updateDto.PhoneNumber ?? profile.PhoneNumber;
            profile.Email = updateDto.Email ?? profile.Email;
            profile.BirthDate = updateDto.BirthDate;
            profile.Ethnicity = updateDto.Ethnicity;
            profile.Religion = updateDto.Religion;
            profile.Nationality = updateDto.Nationality;
            profile.BirthPlace = updateDto.BirthPlace;
            profile.CitizenID = updateDto.CitizenID;
            profile.CitizenIDIssueDate = updateDto.CitizenIDIssueDate;
            profile.CitizenIDIssuePlace = updateDto.CitizenIDIssuePlace;
            profile.PermanentAddress = updateDto.PermanentAddress;
            profile.TemporaryAddress = updateDto.TemporaryAddress;

            // 4. Cập nhật bảng AcademicProfile
            academic.Status = updateDto.Status ?? academic.Status;
            academic.AdmissionDate = updateDto.AdmissionDate ?? academic.AdmissionDate;
            academic.AcademicYear = updateDto.AcademicYear ?? academic.AcademicYear;
            academic.EducationLevel = updateDto.EducationLevel ?? academic.EducationLevel;
            academic.EducationType = updateDto.EducationType ?? academic.EducationType;
            academic.FacultyName = updateDto.FacultyName ?? academic.FacultyName;
            academic.MajorName = updateDto.MajorName ?? academic.MajorName;
            academic.SpecializationName = updateDto.SpecializationName;
            academic.CampusName = updateDto.CampusName ?? academic.CampusName;
            academic.ClassName = updateDto.ClassName ?? academic.ClassName;

            // 5. Cập nhật bảng FamilyRelationship (Logic Xóa cũ - Thêm mới)
            // Lấy danh sách hiện có trong database của sinh viên này
            var existingFamilyMembers = await _context.FamilyRelationships
                .Where(f => f.StudentID == studentid)
                .ToListAsync();

            // Xóa toàn bộ dữ liệu cũ
            _context.FamilyRelationships.RemoveRange(existingFamilyMembers);

            // Thêm mới toàn bộ dữ liệu từ DTO
            if (updateDto.FamilyRelationships != null && updateDto.FamilyRelationships.Any())
            {
                foreach (var memberDto in updateDto.FamilyRelationships)
                {
                    var newMember = new FamilyRelationship
                    {
                        StudentID = studentid, // Gán khóa ngoại
                        RelativeName = memberDto.RelativeName,
                        RelationshipType = memberDto.RelationshipType,
                        PhoneNumber = string.IsNullOrWhiteSpace(memberDto.PhoneNumber) ? null : memberDto.PhoneNumber,
                        BirthYear = memberDto.BirthYear
                        // Không cần gán FamilyMemberID vì database sẽ tự sinh ID mới khi lưu
                    };
                    _context.FamilyRelationships.Add(newMember);
                }
            }

            // 6. Lưu và xác nhận
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return NoContent(); // 204 No Content là chuẩn cho PUT thành công
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Lỗi cập nhật hệ thống: {ex.Message}");
        }
    }

    // POST: api/Student
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<StudentDetailDTO>> PostStudent([FromBody] StudentDetailDTO createDto)
    {
        if (createDto == null) return BadRequest("Dữ liệu không hợp lệ.");

        // Sử dụng Transaction để đảm bảo tính toàn vẹn dữ liệu
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Kiểm tra tồn tại
            if (await _context.Students.AnyAsync(s => s.StudentID == createDto.StudentID))
                return BadRequest("Mã số sinh viên này đã tồn tại.");

            // 2. Student chính
            Student student = new()
            {
                StudentID = createDto.StudentID,
                StudentName = createDto.StudentName,
                Gender = createDto.Gender
            };

            // 3. StudentProfile
            StudentProfile profile = new()
            {
                StudentID = student.StudentID,
                BirthDate = createDto.BirthDate,
                PhoneNumber = createDto.PhoneNumber,
                Email = createDto.Email,
                Ethnicity = createDto.Ethnicity,
                Religion = createDto.Religion,
                Nationality = createDto.Nationality,
                BirthPlace = createDto.BirthPlace,
                CitizenID = createDto.CitizenID,
                CitizenIDIssueDate = createDto.CitizenIDIssueDate,
                CitizenIDIssuePlace = createDto.CitizenIDIssuePlace,
                PermanentAddress = createDto.PermanentAddress,
                TemporaryAddress = createDto.TemporaryAddress
            };

            // 4. AcademicProfile
            AcademicProfile academic = new()
            {
                StudentID = student.StudentID,
                Status = createDto.Status ?? "Đang học",
                AdmissionDate = createDto.AdmissionDate,
                AcademicYear = createDto.AcademicYear,
                EducationLevel = createDto.EducationLevel,
                EducationType = createDto.EducationType,
                FacultyName = createDto.FacultyName,
                MajorName = createDto.MajorName,
                SpecializationName = createDto.SpecializationName,
                CampusName = createDto.CampusName,
                ClassName = createDto.ClassName
            };

            _context.Students.Add(student);
            _context.StudentProfiles.Add(profile);
            _context.AcademicProfiles.Add(academic);

            // 5. Cập nhật bảng FamilyRelationship (Logic Xóa cũ - Thêm mới)
            // Thêm mới toàn bộ dữ liệu từ DTO
            if (createDto.FamilyRelationships != null && createDto.FamilyRelationships.Any())
            {
                foreach (var memberDto in createDto.FamilyRelationships)
                {
                    var newMember = new FamilyRelationship
                    {
                        StudentID = student.StudentID, // Gán khóa ngoại
                        RelativeName = memberDto.RelativeName,
                        RelationshipType = memberDto.RelationshipType,
                        PhoneNumber = string.IsNullOrWhiteSpace(memberDto.PhoneNumber) ? null : memberDto.PhoneNumber,
                        BirthYear = memberDto.BirthYear
                        // Không cần gán FamilyMemberID vì database sẽ tự sinh ID mới khi lưu
                    };
                    _context.FamilyRelationships.Add(newMember);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return CreatedAtAction(nameof(GetStudent), new { studentid = student.StudentID }, createDto);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
        }
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
