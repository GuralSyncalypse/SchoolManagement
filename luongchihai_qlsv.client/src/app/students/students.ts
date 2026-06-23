import { Component, OnInit, signal, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { Student } from '../models';
import { StudentService } from './students.service';

@Component({
  selector: 'app-students',
  standalone: true,
  imports: [DatePipe, RouterModule],
  templateUrl: './students.html',
  styleUrls: ['./students.css']
})
export class Students implements OnInit {
  // Inject Service vào component
  private studentService = inject(StudentService);
  private router = inject(Router);

  // Quản lý danh sách sinh viên bằng Signal (khớp với template students() ở câu hỏi trước)
  students = signal<Student[]>([]);

  ngOnInit(): void {
    // Tải thông tin sinh viên ngay khi khởi tạo
    this.loadStudents();
  }

  // [READ] Gọi API GET: api/Students để lấy dữ liệu đổ vào bảng
  loadStudents(): void {
    this.studentService.getStudents().subscribe({
      next: (data) => this.students.set(data),
      error: (err) => console.error('Lỗi khi tải danh sách sinh viên:', err)
    });
  }

  onView(student: Student): void {
    this.router.navigate(['/students/view', student.studentID])
  }

  // [CREATE] Gọi API POST: api/Students
  onCreate(): void {
    this.router.navigate(['/students/create']);
  }

  // [UPDATE] Gọi API PUT: api/Students/{id}
  onUpdate(student: Student): void {
    this.router.navigate(['/students/edit', student.studentID]);
  }

  // [DELETE] Gọi API DELETE: api/Students/{id}
  onDelete(studentID: string): void {
    if (confirm(`Bạn có chắc chắn muốn xóa sinh viên mã ${studentID}?`)) {
      this.studentService.deleteStudent(studentID).subscribe({
        next: () => {
          alert('Xóa thành công!');
          // Cập nhật nhanh Signal ở Client mà không cần gọi lại API loadStudents() giúp tăng trải nghiệm khách hàng
          this.students.update(list => list.filter(s => s.studentID !== studentID));
        },
        error: (err) => alert('Lỗi xóa sinh viên: ' + (err.error?.message || 'Không thể xóa'))
      });
    }
  }
}
