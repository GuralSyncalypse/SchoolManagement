import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { Course } from '../models';
import { CourseService } from './courses.service';

@Component({
  selector: 'app-courses',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './courses.html',
  styleUrl: './courses.css',
})
export class Courses implements OnInit {
  // Thực hiện cơ chế nhúng dịch vụ (Dependency Injection) bằng hàm inject()
  private courseService = inject(CourseService);
  private router = inject(Router);

  // Quản lý danh sách môn học bằng cấu trúc tín hiệu dữ liệu (Signal) với kiểu mảng Course
  courses = signal<Course[]>([]);

  ngOnInit(): void {
    this.loadCourses();
  }

  // [READ] Truy vấn danh sách môn học từ tầng API Backend ASP.NET Core 8
  loadCourses(): void {
    this.courseService.getCourses().subscribe({
      next: (data) => this.courses.set(data),
      error: (err) => console.error('Hệ thống gặp lỗi khi tải danh sách môn học:', err)
    });
  }

  // [CREATE] Kích hoạt điều hướng giao diện sang trang biểu mẫu thêm mới
  onCreate(): void {
    // Hệ thống chuyển hướng URL sang tuyến đường cấu hình form mà không dùng prompt thủ công
    this.router.navigate(['/courses/create']);
  }

  // [UPDATE] Điều hướng sang trang biểu mẫu kèm theo mã định danh ID
  onUpdate(course: Course): void {
    // Chuyển hướng URL sang định dạng: /courses/edit/1, /courses/edit/2...
    this.router.navigate(['/courses/edit', course.courseID]);
  }

  // [DELETE] Gửi yêu cầu gỡ bỏ bản ghi môn học khỏi hệ thống
  onDelete(courseID: number): void {
    // Khóa chính courseID được định kiểu number để khớp với thuộc tính tự tăng của DB
    if (confirm(`Hệ thống xác nhận việc xóa bỏ môn học có mã định danh: #${courseID}?`)) {
      this.courseService.deleteCourse(courseID).subscribe({
        next: () => {
          alert('Hệ thống đã gỡ bỏ môn học thành công.');
          // Loại bỏ phần tử trực tiếp trên bộ nhớ nội tại của Signal để tối ưu hóa hiệu năng giao diện
          this.courses.update(list => list.filter(c => c.courseID !== courseID));
        },
        error: (err) => alert('Lỗi hệ thống: ' + (err.error?.message || 'Không thể thực thi yêu cầu xóa.'))
      });
    }
  }
}
