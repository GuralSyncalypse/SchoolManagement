import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule, FormArray } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { StudentDetailDTO, FamilyMemberDTO } from '../../models';
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

  // Định nghĩa form theo 4 khối logic
  studentForm!: FormGroup;

  ngOnInit(): void {
    this.initForm();

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.isViewMode.set(this.router.url.includes('/view/'));
      this.isEditMode.set(!this.isViewMode());
      this.studentForm.get('student.studentID')?.disable();

      this.studentService.getStudentById(idParam).subscribe({
        next: (student) => this.patchStudentData(student),
        error: (err) => console.error('Lỗi tải dữ liệu:', err)
      });
    }
  }

  private createFamilyGroup(member?: FamilyMemberDTO): FormGroup {
    return new FormGroup({
      familyMemberID: new FormControl(member?.familyMemberID || 0),
      relativeName: new FormControl(member?.relativeName || '', Validators.required),
      relationshipType: new FormControl(member?.relationshipType || '', Validators.required),
      phoneNumber: new FormControl(member?.phoneNumber || '', ),
      birthYear: new FormControl(member?.birthYear || new Date().getFullYear(), Validators.required)
    });
  }

  private initForm(): void {
      this.studentForm = new FormGroup({
        student: new FormGroup({
          studentID: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.minLength(5)] }),
          studentName: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.minLength(2)] }),
          gender: new FormControl('Nam', { nonNullable: true, validators: Validators.required })
        }),
      profile: new FormGroup({
        birthDate: new FormControl('', { nonNullable: true}),
        birthPlace: new FormControl(''),
        ethnicity: new FormControl('Kinh'),
        religion: new FormControl('Không'),
        nationality: new FormControl('Việt Nam'),
        citizenID: new FormControl('', [Validators.maxLength(12)]),
        citizenIDIssueDate: new FormControl(''),
        citizenIDIssuePlace: new FormControl(''),
        phoneNumber: new FormControl('', { nonNullable: false, validators: Validators.required }),
        email: new FormControl('', { nonNullable: false, validators: Validators.required }),
        permanentAddress: new FormControl(''),
        temporaryAddress: new FormControl('')
      }),
      academic: new FormGroup({
        className: new FormControl(''),
        status: new FormControl('Đang học'),
        facultyName: new FormControl(''),
        majorName: new FormControl(''),
        specializationName: new FormControl(''),
        educationLevel: new FormControl('Đại học'),
        educationType: new FormControl('Chính quy'),
        academicYear: new FormControl(new Date().getFullYear()),
        admissionDate: new FormControl(''),
        campusName: new FormControl('')
      }),
      familyRelationships: new FormArray([])
    });
  }

  private patchStudentData(s: StudentDetailDTO): void {
    // 1. Patch các group đơn lẻ
    this.studentForm.patchValue({
      student: { studentID: s.studentID, studentName: s.studentName, gender: s.gender || 'Nam' },
      profile: { ...s, birthDate: s.birthDate?.toString().substring(0, 10), citizenIDIssueDate: s.citizenIDIssueDate?.toString().substring(0, 10) },
      academic: { ...s }
    });

    // 2. Xử lý Family Array
    this.familyArray.clear(); // Quan trọng: Xóa dữ liệu cũ trước khi push mới
    if (s.familyRelationships && s.familyRelationships.length > 0) {
      s.familyRelationships.forEach(member => {
        this.familyArray.push(this.createFamilyGroup(member));
      });
    }
  }

  get familyArray(): FormArray {
    return this.studentForm.get('familyRelationships') as FormArray;
  }

  addFamilyMember(): void {
    this.familyArray.push(this.createFamilyGroup());
  }

  // Xóa dòng theo index
  removeFamilyMember(index: number): void {
    this.familyArray.removeAt(index);
  }

  onSubmit(): void {
    console.log("Đã nhấn nút lưu, form hiện tại:", this.studentForm.value); // Thêm dòng này
    if (this.studentForm.invalid) {
      console.warn("Form không hợp lệ:", this.studentForm.errors);
      this.studentForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    const raw = this.studentForm.getRawValue();

    // Gom phẳng dữ liệu thành StudentDetailDTO
    const studentData: StudentDetailDTO = {
      ...raw.student,
      ...raw.profile,
      ...raw.academic,

      // Ép kiểu
      phoneNumber: raw.profile.phoneNumber || null,
      birthDate: raw.profile.birthDate || null,
      birthPlace: raw.profile.birthPlace || null,
      citizenID: raw.profile.citizenID || null,
      citizenIDIssueDate: raw.profile.citizenIDIssueDate || null,
      citizenIDIssuePlace: raw.profile.citizenIDIssuePlace || null,
      permanentAddress: raw.profile.permanentAddress || null,
      temporaryAddress: raw.profile.temporaryAddress || null,
      admissionDate: raw.academic.admissionDate || null,

      familyRelationships: raw.familyRelationships // Lấy mảng từ FormArray
    };

    const action = this.isEditMode()
      ? this.studentService.updateStudent(studentData.studentID, studentData)
      : this.studentService.createStudent(studentData);

    action.subscribe({
      next: () => {
        alert('Thành công!');
        this.router.navigate(['/students']);
      },
      error: (err) => this.handleError(err)
    });
  }

  private handleError(err: any): void {
    alert('Lỗi: ' + (err.error?.message || 'Có lỗi xảy ra'));
    this.isSubmitting.set(false);
  }
}
