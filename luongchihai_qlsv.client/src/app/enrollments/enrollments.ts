import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { Enrollment } from '../models';
import { EnrollmentService } from './enrollments.service';

@Component({
  selector: 'app-enrollments',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './enrollments.html'
})
export class Enrollments implements OnInit {
  private enrollmentService = inject(EnrollmentService);
  private router = inject(Router);

  enrollments = signal<Enrollment[]>([]);

  ngOnInit(): void {
    this.loadEnrollments();
  }

  loadEnrollments(): void {
    this.enrollmentService.getEnrollments().subscribe({
      next: (data) => this.enrollments.set(data),
      error: (err) => console.error('Lỗi khi tải danh sách đăng ký:', err)
    });
  }

  onCreate(): void {
    this.router.navigate(['/enrollments/create']);
  }

  // Cập nhật: Nhận 2 tham số thay vì 1 enrollmentID
  onUpdate(studentId: string, courseId: number): void {
    this.router.navigate(['/enrollments/edit', studentId, courseId]);
  }

  // Cập nhật: Nhận 2 tham số để xác định bản ghi cần xóa
  onDelete(studentId: string, courseId: number): void {
    if (confirm(`Xác nhận hủy đăng ký môn học ${courseId} của SV ${studentId}?`)) {
      this.enrollmentService.deleteEnrollment(studentId, courseId).subscribe({
        next: () => {
          // Cập nhật lại signal bằng cách loại bỏ phần tử có cặp khóa trùng khớp
          this.enrollments.update(list =>
            list.filter(e => !(e.studentID === studentId && e.courseID === courseId))
          );
        },
        error: (err) => alert('Không thể xóa lượt đăng ký.')
      });
    }
  }
}
