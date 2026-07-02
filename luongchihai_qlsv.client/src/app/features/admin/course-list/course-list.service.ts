import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Course } from '../../../core/models/course.model';

@Injectable({
  providedIn: 'root'
})
export class AdminCourseService {
  private http = inject(HttpClient)
  private apiUrl = '/api/AdminCourses';

  // GET ALL
  getCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(this.apiUrl);
  }

  // GET BY ID
  getCourse(id: number): Observable<Course> {
    return this.http.get<Course>(`${this.apiUrl}/${id}`);
  }

  // CREATE
  createCourse(course: Course): Observable<Course> {
    return this.http.post<Course>(this.apiUrl, course);
  }

  // UPDATE
  updateCourse(id: number, course: Course): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, course);
  }

  // DELETE
  deleteCourse(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
