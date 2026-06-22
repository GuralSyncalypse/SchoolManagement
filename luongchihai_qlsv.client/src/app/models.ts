
export interface Enrollment {
  enrollmentID: number;
  studentID: string;
  courseID: string;
  grade?: number; // Ví dụ về điểm số (có thể chưa có điểm nên dùng ?)
}

export interface Student {
  studentID: string;       // Không dùng dấu ? vì đây là Khoá chính (Primary Key)
  studentName: string;
  birthDate?: string;      // DateTime trong C# khi chuyển sang JSON qua API sẽ thành chuỗi (String) dạng ISO
  gender?: string;         // Dấu ? tương ứng với string? bên C#
  email?: string;          // Dấu ? tương ứng với string? bên C#

  // Navigation property: Danh sách các lượt đăng ký học phần của sinh viên này
  enrollments?: Enrollment[];
}

export interface Course {
  courseID: number;       // Không dùng dấu ? vì đây là Khoá chính (Primary Key)
  courseName: string;
  courseCode: string;
  credits?: number;

  // Navigation property: Danh sách các lượt đăng ký học phần của sinh viên này
  enrollments?: Enrollment[];
}

