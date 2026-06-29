import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { EnrollmentService } from '../enrollments.service';
import { EnrollmentRequest } from '../../models';
import { Observable } from 'rxjs'; // Đảm bảo đã import

@Component({
  selector: 'app-enrollment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './form.html',
  styleUrls: ['./form.css']
})
export class EnrollmentForm implements OnInit {
  private service = inject(EnrollmentService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  isSubmitting = signal(false);
  isEditMode = signal(false);
  currentEnrollmentID = signal<number | null>(null); // Lưu ID để dùng khi Update

  // Trong class EnrollmentForm
  academicYears = [new Date().getFullYear()]; // Nếu bạn muốn chọn nhiều năm, có thể tạo hàm generate danh sách năm
  semesters = [
    { id: 1, label: 'Học kỳ 1' },
    { id: 2, label: 'Học kỳ 2' },
    { id: 3, label: 'Học kỳ Hè' }
  ];

  form = new FormGroup({
    enrollmentID: new FormControl<number>(0), // Thêm ID vào form
    studentID: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    courseID: new FormControl<number | null>(null, { validators: [Validators.required] }),
    academicYear: new FormControl<number>(2025, { nonNullable: true, validators: [Validators.required] }),
    semester: new FormControl<number>(1, { nonNullable: true, validators: [Validators.required] }),
    processScore: new FormControl<number | null>(null, [Validators.min(0), Validators.max(10)]),
    midtermScore: new FormControl<number | null>(null, [Validators.min(0), Validators.max(10)]),
    finalExamScore: new FormControl<number | null>(null, [Validators.min(0), Validators.max(10)])
  });

  // Hàm dùng chung để tự động sửa điểm khi nhập tay quá khoảng
  onScoreInput(controlName: 'processScore' | 'midtermScore' | 'finalExamScore'): void {
    const control = this.form.get(controlName);

    if (control && control.value !== null) {
      let val = control.value;

      if (val > 10) {
        control.setValue(10, { emitEvent: false });
      } else if (val < 0) {
        control.setValue(0, { emitEvent: false });
      }
    }
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id'); // Lấy ID từ URL: /edit/:id

    if (id) {
      this.isEditMode.set(true);
      this.currentEnrollmentID.set(+id);

      this.service.getEnrollment(+id).subscribe(data => {
        this.form.patchValue(data);
        academicYear: new Date().getFullYear()

        // Có thể disable các trường khóa nếu không muốn cho sửa
        this.form.controls.studentID.disable();
        this.form.controls.courseID.disable();
        this.form.controls.academicYear.disable();
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.isSubmitting.set(true);

    const formData = this.form.getRawValue();

    // Ép kiểu tường minh cho action$ là một Observable
    const action$: Observable<any> = this.isEditMode()
      ? this.service.updateEnrollment(this.currentEnrollmentID()!, formData as EnrollmentRequest)
      : this.service.createEnrollment(formData as EnrollmentRequest);

    action$.subscribe({
      next: () => this.router.navigate(['/enrollments']),
      error: (err) => {
        console.error(err);
        if (err.error?.errors) {
          this.handleError(err.error.errors);
        }
        this.isSubmitting.set(false);
      }
    });
  }

  handleError(errors: any) {
    Object.keys(errors).forEach(key => {
      // Tìm control tương ứng và set lỗi
      const control = this.form.get(key.charAt(0).toLowerCase() + key.slice(1));
      control?.setErrors({ serverError: errors[key][0] });
    });
  }
}
