import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StudentListDTO, StudentDetailDTO } from '../models/student.model';


@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private http = inject(HttpClient);

  // Thay đổi port này trùng với port dự án ASP.NET Core của bạn
  private apiUrl = '/api/students';

  // GET: api/Students (GetStudent trong Controller)
  getStudents(): Observable<StudentListDTO[]> {
    return this.http.get<StudentListDTO[]>(this.apiUrl);
  }

  // GET: api/Students/{id}
  getStudentById(id: string): Observable<StudentDetailDTO> {
    return this.http.get<StudentDetailDTO>(`${this.apiUrl}/${id}`);
  }

  // POST: api/Students (PostStudent trong Controller)
  createStudent(student: StudentDetailDTO): Observable<StudentDetailDTO> {
    return this.http.post<StudentDetailDTO>(this.apiUrl, student);
  }

  // PUT: api/Students/{id} (PutStudent trong Controller)
  updateStudent(id: string, student: StudentDetailDTO): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, student);
  }

  // DELETE: api/Students/{id} (DeleteStudent trong Controller)
  deleteStudent(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
