import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Student } from '../../models';
import { StudentService } from '../students.service';

@Component({
  selector: 'app-student-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './form.html',
  styleUrl: './form.css'
})
export class StudentForm implements OnInit {
  private studentService = inject(StudentService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  isSubmitting = signal<boolean>(false);
  isEditMode = signal<boolean>(false);

  studentForm = new FormGroup({
    // BỔ SUNG VALIDATION: Mã số sinh viên bắt buộc nhập, tối thiểu 5 ký tự
    studentID: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(5)]
    }),
    studentName: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(2)]
    }),
    birthDate: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required]
    }),
    gender: new FormControl<string>('Nam', { nonNullable: true }),
    email: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.email]
    })
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      this.isEditMode.set(true);

      // XỬ LÝ KHÓA KHÓA CHÍNH: Khóa ô nhập mã số sinh viên khi ở chế độ chỉnh sửa
      this.studentForm.get('studentID')?.disable();

      this.studentService.getStudentById(idParam).subscribe({
        next: (student) => {
          if (student) {
            const formattedDate = student.birthDate ? student.birthDate.substring(0, 10) : '';

            this.studentForm.patchValue({
              studentID: student.studentID,
              studentName: student.studentName,
              birthDate: formattedDate,
              gender: student.gender || 'Nam',
              email: student.email || ''
            });
          }
        },
        error: (err) => console.error('Hệ thống không thể tải thông tin sinh viên:', err)
      });
    }
  }

  onSubmit(): void {
    if (this.studentForm.invalid) {
      this.studentForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);

    // LƯU Ý KỸ THUẬT: getRawValue() sẽ lấy toàn bộ dữ liệu bao gồm cả các ô đã bị disabled (như studentID)
    const rawData = this.studentForm.getRawValue();

    const studentData: Student = {
      studentID: rawData.studentID || '',
      studentName: rawData.studentName,
      birthDate: rawData.birthDate,
      gender: rawData.gender,
      email: rawData.email
    };

    if (this.isEditMode()) {
      this.studentService.updateStudent(studentData.studentID, studentData).subscribe({
        next: () => {
          alert('Hệ thống đã cập nhật thông tin sinh viên thành công.');
          this.router.navigate(['/students']);
        },
        error: (err) => this.handleError(err)
      });
    } else {
      this.studentService.createStudent(studentData).subscribe({
        next: () => {
          alert('Hệ thống đã ghi nhận và lưu trữ thông tin sinh viên thành công.');
          this.router.navigate(['/students']);
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
