
/**
 * DTO used when creating or updating enrollment data
 */
export interface EnrollmentRequest {
  processScore?: number | null;
  midtermScore?: number | null;
  finalExamScore?: number | null;
}

/**
 * DTO used when receiving enrollment data from the API
 */
export interface EnrollmentResponse {
  enrollmentID: number;
  studentID: string;
  offeringID: number;
  processScore?: number | null;
  midtermScore?: number | null;
  finalExamScore?: number | null;
  totalScore?: number | null;
  isPassed?: boolean | null;
}

// 1. Interface phục vụ cho trang hiển thị DANH SÁCH (Gọn nhẹ, load nhanh)
export interface StudentListDTO {
  studentID: string;
  studentName: string;
  gender?: string;
  className: string;   // Khớp với DTO mới
  majorName: string;   // Khớp với DTO mới
  status: string;      // Khớp với DTO mới
  phoneNumber: string; // Khớp với DTO mới
}

// 2. Interface phụ trợ cho danh sách NGƯỜI THÂN trong Form chi tiết
export interface FamilyMemberDTO {
  familyMemberID: number;
  relativeName: string;
  relationshipType: string;
  phoneNumber?: string;   // Dấu ? vì trong SQL cho phép NULL
  birthYear: number;
}

// 3. Interface phục vụ cho trang hiển thị FORM CHI TIẾT (Đầy đủ thông tin đã được phẳng hóa)
export interface StudentDetailDTO {
  // Thông tin cơ bản (Student)
  studentID: string;
  studentName: string;
  gender?: string;

  // Thông tin học vấn (AcademicProfile)
  admissionDate?: string;  // Kiểu DATE/DateTime từ API trả về sẽ là chuỗi ISO string (vd: "2026-06-23T00:00:00")
  className?: string;
  campusName?: string;
  educationLevel?: string;
  educationType?: string;
  facultyName?: string;
  majorName?: string;
  specializationName?: string; // Dấu ? vì SQL cho phép NULL
  academicYear?: number;
  status: string;

  // Thông tin cá nhân (StudentProfile)
  birthDate?: string;
  ethnicity?: string;
  religion?: string;
  nationality?: string;
  birthPlace?: string;
  citizenID?: string;
  citizenIDIssueDate?: string;
  citizenIDIssuePlace?: string;
  phoneNumber: string;
  email: string;
  permanentAddress?: string;
  temporaryAddress?: string;

  // Mối quan hệ 1 - Nhiều (Danh sách người thân)
  familyRelationships?: FamilyMemberDTO[];
}

export interface AcademicYear {
  yearID: number;
  yearName: string;
}

export interface SemesterType {
  typeID: number;
  typeName: string;
}

export interface CourseOffering {
  offeringID: number;

  courseID: number;
  yearID: number;
  typeID: number;

  instructorID?: number | null;

  maxStudents?: number | null;
  currentStudents: number;

  isOpen?: boolean | null;

  // Navigation (optional - chỉ dùng khi API include join)
  course?: Course;
  academicYear?: AcademicYear;
  semesterType?: SemesterType;
}

export interface Course {
  courseID: number;       // Không dùng dấu ? vì đây là Khoá chính (Primary Key)
  courseName: string;
  courseCode: string;
  credits?: number;

  // Navigation property: Danh sách các lượt đăng ký học phần của sinh viên này
  courseOferrings?: CourseOffering[];
}

