import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Course } from '../models';


@Injectable({
  providedIn: 'root'
})
export class CourseService {
  private http = inject(HttpClient);

  // Thay đổi port này trùng với port dự án ASP.NET Core của bạn
  private apiUrl = '/api/courses';

  // GET: api/Courses (GetCourse trong Controller)
  getCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(this.apiUrl);
  }

  // GET: api/Courses/{id}
  getCourseById(id: number): Observable<Course> {
    return this.http.get<Course>(`${this.apiUrl}/${id}`);
  }

  // POST: api/Courses (PostCourse trong Controller)
  createCourse(Course: Course): Observable<Course> {
    return this.http.post<Course>(this.apiUrl, Course);
  }

  // PUT: api/Courses/{id} (PutCourse trong Controller)
  updateCourse(id: number, Course: Course): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, Course);
  }

  // DELETE: api/Courses/{id} (DeleteCourse trong Controller)
  deleteCourse(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
