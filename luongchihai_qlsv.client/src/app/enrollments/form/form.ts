import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Enrollment } from '../../models';
import { EnrollmentService } from '../enrollments.service';

@Component({
  selector: 'app-enrollment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './form.html'
})
export class EnrollmentForm implements OnInit {
  private enrollmentService = inject(EnrollmentService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  isSubmitting = signal<boolean>(false);
  isEditMode = signal<boolean>(false);

  // Không còn enrollmentID, thay bằng cặp khóa chính
  enrollmentForm = new FormGroup({
    studentID: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.maxLength(15)]
    }),
    courseID: new FormControl<number>(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(1)]
    }),
    enrollmentDate: new FormControl<string>(new Date().toISOString().substring(0, 10), {
      nonNullable: true,
      validators: [Validators.required]
    })
  });

  ngOnInit(): void {
    // Để sửa bản ghi có khóa kép, URL cần chứa cả 2 tham số: /edit/:studentId/:courseId
    const studentId = this.route.snapshot.paramMap.get('studentId');
    const courseId = this.route.snapshot.paramMap.get('courseId');

    if (studentId && courseId) {
      this.isEditMode.set(true);

      // Gọi service dùng khóa kép
      this.enrollmentService.getEnrollmentByIDs(studentId, parseInt(courseId, 10)).subscribe({
        next: (data) => {
          this.enrollmentForm.patchValue({
            studentID: data.studentID,
            courseID: data.courseID,
            enrollmentDate: data.enrollmentDate.substring(0, 10)
          });
          // Disable khóa chính khi ở chế độ sửa để tránh thay đổi khóa
          this.enrollmentForm.get('studentID')?.disable();
          this.enrollmentForm.get('courseID')?.disable();
        },
        error: () => alert('Không thể tải thông tin đăng ký.')
      });
    }
  }

  onSubmit(): void {
    if (this.enrollmentForm.invalid) {
      this.enrollmentForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    // getRawValue() sẽ lấy cả các field đã disable
    const rawData = this.enrollmentForm.getRawValue();

    const enrollmentData: Enrollment = {
      studentID: rawData.studentID,
      courseID: rawData.courseID,
      enrollmentDate: rawData.enrollmentDate
    };

    if (this.isEditMode()) {
      // Gọi update với 2 tham số
      this.enrollmentService.updateEnrollment(enrollmentData.studentID, enrollmentData.courseID, enrollmentData).subscribe({
        next: () => {
          alert('Cập nhật đăng ký thành công.');
          this.router.navigate(['/enrollments']);
        },
        error: (err) => this.handleError(err)
      });
    } else {
      this.enrollmentService.createEnrollment(enrollmentData).subscribe({
        next: () => {
          alert('Đăng ký môn học thành công.');
          this.router.navigate(['/enrollments']);
        },
        error: (err) => this.handleError(err)
      });
    }
  }

  private handleError(err: any): void {
    alert('Lỗi hệ thống: ' + (err.error?.message || 'Không thể hoàn tất thao tác.'));
    this.isSubmitting.set(false);
  }
}
