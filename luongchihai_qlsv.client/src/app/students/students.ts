import { Component, OnInit, signal, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { StudentListDTO } from '../models';
import { StudentService } from './students.service';

@Component({
  selector: 'app-students',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './students.html',
  styleUrls: ['./students.css']
})
export class Students implements OnInit {
  // Inject Service và Router vào component
  private studentService = inject(StudentService);
  private router = inject(Router);

  // Quản lý danh sách sinh viên bằng Signal với kiểu dữ liệu DTO chuẩn mới
  students = signal<StudentListDTO[]>([]);

  ngOnInit(): void {
    // Tải thông tin sinh viên ngay khi khởi tạo
    this.loadStudents();
  }

  // [READ] Gọi API GET để lấy dữ liệu đổ vào bảng
  loadStudents(): void {
    this.studentService.getStudents().subscribe({
      next: (data) => this.students.set(data),
      error: (err) => console.error('Lỗi khi tải danh sách sinh viên:', err)
    });
  }

  // Chuyển sang dùng StudentListDTO
  onView(student: StudentListDTO): void {
    this.router.navigate(['/students/view', student.studentID]);
  }

  // [CREATE] Chuyển hướng sang trang tạo mới
  onCreate(): void {
    this.router.navigate(['/students/create']);
  }

  // [UPDATE] Chuyển sang dùng StudentListDTO
  onUpdate(student: StudentListDTO): void {
    this.router.navigate(['/students/edit', student.studentID]);
  }

  // [DELETE] Gọi API DELETE để xóa sinh viên
  onDelete(studentID: string): void {
    if (confirm(`Bạn có chắc chắn muốn xóa sinh viên mã ${studentID}?`)) {
      this.studentService.deleteStudent(studentID).subscribe({
        next: () => {
          alert('Xóa thành công!');
          // Cập nhật nhanh Signal ở Client giúp giao diện mất đi bản ghi đó ngay lập tức
          this.students.update(list => list.filter(s => s.studentID !== studentID));
        },
        error: (err) => alert('Lỗi xóa sinh viên: ' + (err.error?.message || 'Không thể xóa'))
      });
    }
  }
}
