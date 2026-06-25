import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { EnrollmentService } from './enrollments.service'; // Adjust path
import { EnrollmentResponse, EnrollmentRequest } from '../models';
import { FormatScorePipe } from './format-score.pipe'

@Component({
  selector: 'app-enrollment',
  standalone: true,
  imports: [CommonModule, RouterModule, FormatScorePipe],
  templateUrl: './enrollments.html'
})
export class Enrollments implements OnInit {
  private enrollmentService = inject(EnrollmentService);
  private router = inject(Router)

  enrollments = signal<EnrollmentResponse[]>([]);
  isLoading = signal<boolean>(false);

  ngOnInit() {
    this.loadEnrollments();
  }

  loadEnrollments() {
    this.isLoading.set(true);
    this.enrollmentService.getEnrollments().subscribe({
      next: (data) => {
        this.enrollments.set(data);
        this.isLoading.set(false);
      }
    });
  }

  // Ensure the parameter type is flexible or explicitly handled
  onDelete(studentID: string, courseID: string | number) {
    // Convert courseId to number safely
    const numericCourseID = typeof courseID === 'string' ? parseInt(courseID, 10) : courseID;

    if (confirm('Are you sure you want to delete this record?')) {
      this.enrollmentService.deleteEnrollment(studentID, numericCourseID).subscribe({
        next: () => {
          // Update local state signal
          this.enrollments.update(list =>
            list.filter(e => e.studentID !== studentID || e.courseID !== numericCourseID.toString())
          );
        }
      });
    }
  }

  onCreate() {
    this.router.navigate(['/enrollments/create']);
  }

  onUpdate(studentId: string, courseId: number) {
    // Điều hướng kèm params (khóa kép)
    this.router.navigate(['/enrollments/edit', studentId, courseId]);
  }
}
