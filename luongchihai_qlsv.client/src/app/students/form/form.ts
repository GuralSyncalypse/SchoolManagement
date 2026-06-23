import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { StudentDetailDTO } from '../../models';
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
  isViewMode = signal<boolean>(false);

  // Định nghĩa FormGroup chứa toàn bộ thông tin phẳng của StudentDetailDTO
  studentForm = new FormGroup({
    // ==========================================
    // 👤 NHÓM THÔNG TIN CƠ BẢN (BẮT BUỘC PHẢI CÓ)
    // ==========================================
    studentID: new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.minLength(5)] }),
    studentName: new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.minLength(2)] }),
    gender: new FormControl<string>('Nam', { nonNullable: true }), // Mặc định là Nam, không sợ trống
    birthDate: new FormControl<string>('', { nonNullable: true, validators: [Validators.required] }),
    phoneNumber: new FormControl<string>('', { nonNullable: true, validators: [Validators.required] }),
    email: new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.email] }),

    // ==========================================
    // 🎓 NHÓM THÔNG TIN HỌC VẤN (BỔ SUNG SAU - KHÔNG validator)
    // ==========================================
    className: new FormControl<string>('', { nonNullable: true }),
    status: new FormControl<string>('Đang học', { nonNullable: true }),
    facultyName: new FormControl<string>('', { nonNullable: true }),
    majorName: new FormControl<string>('', { nonNullable: true }),
    specializationName: new FormControl<string>('', { nonNullable: true }),
    educationLevel: new FormControl<string>('Đại học', { nonNullable: true }),
    educationType: new FormControl<string>('Chính quy', { nonNullable: true }),
    academicYear: new FormControl<number>(new Date().getFullYear(), { nonNullable: true }),
    admissionDate: new FormControl<string>('', { nonNullable: true }),
    campusName: new FormControl<string>('', { nonNullable: true }),

    // ==========================================
    // 📝 NHÓM THÔNG TIN ĐỊNH DANH (BỔ SUNG SAU - KHÔNG validator)
    // ==========================================
    birthPlace: new FormControl<string>('', { nonNullable: true }),
    citizenID: new FormControl<string>('', { nonNullable: true, validators: [Validators.maxLength(12)] }), // Chỉ check nếu có gõ
    citizenIDIssueDate: new FormControl<string>('', { nonNullable: true }),
    citizenIDIssuePlace: new FormControl<string>('', { nonNullable: true }),
    ethnicity: new FormControl<string>('Kinh', { nonNullable: true }),
    religion: new FormControl<string>('Không', { nonNullable: true }),
    nationality: new FormControl<string>('Việt Nam', { nonNullable: true }),
    permanentAddress: new FormControl<string>('', { nonNullable: true }),
    temporaryAddress: new FormControl<string>('', { nonNullable: true })
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    const isViewRoute = this.router.url.includes('/view/');

    if (idParam) {
      if (isViewRoute) {
        this.isViewMode.set(true);
      } else {
        this.isEditMode.set(true);
      }

      this.studentForm.get('studentID')?.disable();

      this.studentService.getStudentById(idParam).subscribe({
        next: (student) => {
          if (student) {
            // Định dạng chuỗi ngày YYYY-MM-DD để hiển thị chuẩn trên thẻ <input type="date">
            const fmtBirthDate = student.birthDate ? student.birthDate.substring(0, 10) : '';
            const fmtAdmissionDate = student.admissionDate ? student.admissionDate.substring(0, 10) : '';
            const fmtCitizenDate = student.citizenIDIssueDate ? student.citizenIDIssueDate.substring(0, 10) : '';

            // Đổ toàn bộ dữ liệu nhận từ DTO vào Form
            this.studentForm.patchValue({
              studentID: student.studentID,
              studentName: student.studentName,
              gender: student.gender || 'Nam',

              admissionDate: fmtAdmissionDate,
              className: student.className,
              campusName: student.campusName,
              educationLevel: student.educationLevel,
              educationType: student.educationType,
              facultyName: student.facultyName,
              majorName: student.majorName,
              specializationName: student.specializationName || '',
              academicYear: student.academicYear,
              status: student.status,

              birthDate: fmtBirthDate,
              ethnicity: student.ethnicity,
              religion: student.religion,
              nationality: student.nationality,
              birthPlace: student.birthPlace,
              citizenID: student.citizenID,
              citizenIDIssueDate: fmtCitizenDate,
              citizenIDIssuePlace: student.citizenIDIssuePlace,
              phoneNumber: student.phoneNumber,
              email: student.email,
              permanentAddress: student.permanentAddress,
              temporaryAddress: student.temporaryAddress
            });

            if (this.isViewMode()) {
              this.studentForm.disable(); // Xem chi tiết: Khóa tất cả các ô nhập liệu
            } else {
              this.studentForm.get('studentID')?.disable(); // Chỉnh sửa: Chỉ khóa Mã SV
            }
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

    // Lấy toàn bộ giá trị (Kể cả các trường bị disabled)
    const rawData = this.studentForm.getRawValue();

    // Map chuẩn xác từng trường tương ứng với cấu trúc StudentDetailDTO
    const studentData: StudentDetailDTO = {
      studentID: rawData.studentID,
      studentName: rawData.studentName,
      gender: rawData.gender,

      admissionDate: rawData.admissionDate,
      className: rawData.className,
      campusName: rawData.campusName,
      educationLevel: rawData.educationLevel,
      educationType: rawData.educationType,
      facultyName: rawData.facultyName,
      majorName: rawData.majorName,
      specializationName: rawData.specializationName || undefined,
      academicYear: Number(rawData.academicYear),
      status: rawData.status,

      birthDate: rawData.birthDate,
      ethnicity: rawData.ethnicity,
      religion: rawData.religion,
      nationality: rawData.nationality,
      birthPlace: rawData.birthPlace,
      citizenID: rawData.citizenID,
      citizenIDIssueDate: rawData.citizenIDIssueDate,
      citizenIDIssuePlace: rawData.citizenIDIssuePlace,
      phoneNumber: rawData.phoneNumber,
      email: rawData.email,
      permanentAddress: rawData.permanentAddress,
      temporaryAddress: rawData.temporaryAddress,

      // Tạm thời khởi tạo mảng rỗng cho phần người thân (Family Relationship) 
      // để khớp interface, bạn có thể thiết lập FormArray xử lý phần này sau.
      familyRelationships: []
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
