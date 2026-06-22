import { Component, OnInit, signal, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Student } from '../models';
import { StudentService } from './students.service';

@Component({
  selector: 'app-students',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './students.html',
  styleUrls: ['./students.css']
})
export class Students implements OnInit {
  // Inject Service vào component
  private studentService = inject(StudentService);

  // Quản lý danh sách sinh viên bằng Signal (khớp với template students() ở câu hỏi trước)
  students = signal<Student[]>([]);

  ngOnInit(): void {
    this.loadStudents();
  }

  // [READ] Gọi API GET: api/Students để lấy dữ liệu đổ vào bảng
  loadStudents(): void {
    this.studentService.getStudents().subscribe({
      next: (data) => this.students.set(data),
      error: (err) => console.error('Lỗi khi tải danh sách sinh viên:', err)
    });
  }

  // [CREATE] Gọi API POST: api/Students
  onCreate(): void {
    // Để ví dụ đơn giản, ta dùng prompt thu thập dữ liệu nhanh. 
    // Trong thực tế, đoạn này bạn sẽ mở một Modal Form hoặc điều hướng trang hướng dẫn điền form.
    const studentID = prompt('Nhập mã sinh viên mới:');
    const studentName = prompt('Nhập tên sinh viên:');

    if (!studentID || !studentName) return;

    const newStudent: Student = {
      studentID: studentID,
      studentName: studentName,
      birthDate: new Date().toISOString().split('T')[0], // Ngày hiện tại định dạng YYYY-MM-DD
      gender: 'Nam',
      email: `${studentID.toLowerCase()}@school.edu.vn`
    };

    this.studentService.createStudent(newStudent).subscribe({
      next: (res) => {
        alert('Thêm mới thành công!');
        this.loadStudents(); // Tải lại danh sách mới từ DB
      },
      error: (err) => alert('Lỗi: ' + (err.error?.message || 'Không thể thêm sinh viên'))
    });
  }

  // [UPDATE] Gọi API PUT: api/Students/{id}
  onUpdate(student: Student): void {
    const newName = prompt('Cập nhật lại tên sinh viên:', student.studentName);
    if (newName === null) return; // Nhấn Cancel bỏ qua

    // Tạo bản sao đối tượng dữ liệu được chỉnh sửa
    const updatedStudent: Student = { ...student, studentName: newName };

    this.studentService.updateStudent(student.studentID, updatedStudent).subscribe({
      next: () => {
        alert('Cập nhật thành công!');
        this.loadStudents(); // Làm mới danh sách
      },
      error: (err) => alert('Lỗi cập nhật: ' + (err.error?.message || 'Có lỗi xảy ra'))
    });
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
