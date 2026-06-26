import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EnrollmentResponse, EnrollmentRequest } from '../models'; // Import interface EnrollmentResponse

@Injectable({ providedIn: 'root' })
export class EnrollmentService {
  private http = inject(HttpClient);
  private apiUrl = '/api/enrollments';

  // Lấy danh sách toàn bộ đăng ký
  getEnrollments(): Observable<EnrollmentResponse[]> {
    return this.http.get<EnrollmentResponse[]>(this.apiUrl);
  }

  // Lấy chi tiết một đăng ký bằng khóa kép
  getEnrollmentByIDs(studentID: string, courseID: number): Observable<EnrollmentResponse> {
    return this.http.get<EnrollmentResponse>(`${this.apiUrl}/${studentID}/${courseID}`);
  }

  // Thêm mới một đăng ký
  createEnrollment(enrollment: EnrollmentResponse): Observable<EnrollmentResponse> {
    return this.http.post<EnrollmentResponse>(this.apiUrl, enrollment);
  }

  // Cập nhật đăng ký bằng khóa kép
  // Lưu ý: Cần truyền đầy đủ tham số để API định danh đúng bản ghi cần sửa
  updateEnrollment(studentID: string, courseID: number, enrollment: EnrollmentResponse): Observable<any> {
    return this.http.put(`${this.apiUrl}/${studentID}/${courseID}`, enrollment);
  }

  // Xóa đăng ký bằng khóa kép
  deleteEnrollment(studentID: string, courseID: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${studentID}/${courseID}`);
  }
}
