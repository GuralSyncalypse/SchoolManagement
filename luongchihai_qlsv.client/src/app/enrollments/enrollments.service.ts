import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Enrollment } from '../models';

@Injectable({ providedIn: 'root' })
export class EnrollmentService {
  private http = inject(HttpClient);
  private apiUrl = '/api/enrollments';

  // Lấy danh sách toàn bộ đăng ký
  getEnrollments(): Observable<Enrollment[]> {
    return this.http.get<Enrollment[]>(this.apiUrl);
  }

  // Lấy chi tiết một đăng ký bằng khóa kép
  getEnrollmentByIDs(studentId: string, courseId: number): Observable<Enrollment> {
    return this.http.get<Enrollment>(`${this.apiUrl}/${studentId}/${courseId}`);
  }

  // Thêm mới một đăng ký
  createEnrollment(enrollment: Enrollment): Observable<Enrollment> {
    return this.http.post<Enrollment>(this.apiUrl, enrollment);
  }

  // Cập nhật đăng ký bằng khóa kép
  // Lưu ý: Cần truyền đầy đủ tham số để API định danh đúng bản ghi cần sửa
  updateEnrollment(studentId: string, courseId: number, enrollment: Enrollment): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${studentId}/${courseId}`, enrollment);
  }

  // Xóa đăng ký bằng khóa kép
  deleteEnrollment(studentId: string, courseId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${studentId}/${courseId}`);
  }
}
