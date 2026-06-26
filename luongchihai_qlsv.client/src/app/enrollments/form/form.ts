import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EnrollmentService } from '../enrollments.service';
import { EnrollmentRequest } from '../../models';

@Component({
  selector: 'app-enrollment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './form.html',
  styleUrls: ['./form.css']
})
export class EnrollmentForm implements OnInit {
  private service = inject(EnrollmentService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  isSubmitting = signal(false);
  isEditMode = signal(false);

  form = new FormGroup({
    studentID: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    courseID: new FormControl<number | null>(null, { validators: [Validators.required] }),
    semester: new FormControl('', { nonNullable: true, validators: [Validators.required] }),

    // Bổ sung Validator min và max vào đây
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
    const sId = this.route.snapshot.paramMap.get('studentId');
    const cId = this.route.snapshot.paramMap.get('courseId');

    if (sId && cId) {
      this.isEditMode.set(true);
      this.service.getEnrollmentByIDs(sId, +cId).subscribe(data => {
        this.form.patchValue(data);
        this.form.controls.studentID.disable();
        this.form.controls.courseID.disable();
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.isSubmitting.set(true);

    // Sử dụng getRawValue để lấy cả giá trị của các trường bị disabled
    const formData = this.form.getRawValue();

    // Đảm bảo dữ liệu gửi đi sạch sẽ
    const payload = {
      ...formData,
      courseID: Number(formData.courseID)
    };

    console.log("Submit!");
    const action$ = this.isEditMode()
      ? this.service.updateEnrollment(payload.studentID, payload.courseID, payload)
      : this.service.createEnrollment(payload);

    action$.subscribe({
      next: () => this.router.navigate(['/enrollments']),
      error: (err) => {
        console.error(err);
        this.isSubmitting.set(false);
      }
    });
  }
}
