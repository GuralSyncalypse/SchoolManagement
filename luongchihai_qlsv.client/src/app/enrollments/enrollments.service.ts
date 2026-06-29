import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EnrollmentRequest, EnrollmentResponse } from '../models';

@Injectable({ providedIn: 'root' })
export class EnrollmentService {
  private http = inject(HttpClient);
  private apiUrl = '/api/enrollments';

  // Lấy danh sách toàn bộ đăng ký
  getEnrollments(): Observable<EnrollmentResponse[]> {
    return this.http.get<EnrollmentResponse[]>(this.apiUrl);
  }

  // Lấy chi tiết một đăng ký bằng EnrollmentID (Đơn giản hơn nhiều!)
  getEnrollment(enrollmentID: number): Observable<EnrollmentResponse> {
    return this.http.get<EnrollmentResponse>(`${this.apiUrl}/${enrollmentID}`);
  }

  // Thêm mới: Truyền EnrollmentRequest (Không chứa ID)
  createEnrollment(enrollment: EnrollmentRequest): Observable<EnrollmentResponse> {
    return this.http.post<EnrollmentResponse>(this.apiUrl, enrollment);
  }

  // Cập nhật: Chỉ cần truyền ID và DTO Update
  updateEnrollment(enrollmentID: number, enrollment: EnrollmentRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/${enrollmentID}`, enrollment);
  }

  // Xóa: Chỉ cần truyền ID
  deleteEnrollment(enrollmentID: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${enrollmentID}`);
  }
}
