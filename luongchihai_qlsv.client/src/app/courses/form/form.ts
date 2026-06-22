import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router'; // Nạp thêm công cụ điều hướng
import { Course } from '../../models';
import { CourseService } from '../courses.service';

@Component({
  selector: 'app-course-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './form.html',
  styleUrl: './form.css'
})
export class CourseForm implements OnInit {
  private courseService = inject(CourseService);
  private route = inject(ActivatedRoute); // Dùng để bóc tách tham số ID trên URL
  private router = inject(Router);

  isSubmitting = signal<boolean>(false);
  isEditMode = signal<boolean>(false); // Cờ đánh dấu chế độ: Thêm mới (false) hoặc Sửa (true)

  courseForm = new FormGroup({
    courseID: new FormControl<number>(0),
    courseName: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(3)]
    }),
    courseCode: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.pattern(/^[A-Z0-9_-]+$/)]
    }),
    credits: new FormControl<number>(3, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(1), Validators.max(10)]
    })
  });

  ngOnInit(): void {
    // Bóc tách tham số 'id' từ thanh URL
    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      const targetId = parseInt(idParam, 10);
      this.isEditMode.set(true); // Kích hoạt trạng thái chỉnh sửa

      // Truy vấn thông tin chi tiết môn học từ API để đổ vào form
      this.courseService.getCourses().subscribe({
        next: (list) => {
          const matchCourse = list.find(c => c.courseID === targetId);
          if (matchCourse) {
            // Đổ dữ liệu cũ vào các ô nhập liệu của Form
            this.courseForm.patchValue({
              courseID: matchCourse.courseID,
              courseName: matchCourse.courseName,
              courseCode: matchCourse.courseCode,
              credits: matchCourse.credits
            });
          }
        }
      });
    }
  }

  onSubmit(): void {
    if (this.courseForm.invalid) {
      this.courseForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    const rawData = this.courseForm.getRawValue();

    const courseData: Course = {
      courseID: rawData.courseID || 0,
      courseName: rawData.courseName,
      courseCode: rawData.courseCode,
      credits: rawData.credits
    };

    // RẼ NHÁNH XỬ LÝ API DỰA VÀO TRẠNG THÁI FORM
    if (this.isEditMode()) {
      // Chế độ chỉnh sửa: Gọi API PUT: api/courses/{id}
      this.courseService.updateCourse(courseData.courseID, courseData).subscribe({
        next: () => {
          alert('Hệ thống đã cập nhật thông tin môn học thành công.');
          this.router.navigate(['/courses']); // Quay về trang danh sách
        },
        error: (err) => this.handleError(err)
      });
    } else {
      // Chế độ thêm mới: Gọi API POST: api/courses
      this.courseService.createCourse(courseData).subscribe({
        next: () => {
          alert('Hệ thống đã ghi nhận và lưu trữ môn học thành công.');
          this.router.navigate(['/courses']);
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
