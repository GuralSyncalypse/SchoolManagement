import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { EnrollmentService } from '../enrollments.service';
import { EnrollmentRequest } from '../../models';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Observable } from 'rxjs'; // Đảm bảo đã import

@Component({
  selector: 'app-enrollment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './form.html',
  styleUrls: ['./form.css']
})
export class EnrollmentForm implements OnInit {
  private http = inject(HttpClient);
  private service = inject(EnrollmentService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  isSubmitting = signal(false);
  isEditMode = signal(false);
  currentEnrollmentID = signal<number | null>(null); // Lưu ID để dùng khi Update

  offerings: any[] = [];

  loadOfferings(studentId: string) {
    const params = new HttpParams()
      .set('studentId', studentId)
      .set('yearId', 1)
      .set('typeId', 1);

    this.http.get<any[]>('/api/courseofferings/available', { params })
      .subscribe(data => {
        this.offerings = data;
      });
  }

  form = new FormGroup({
    enrollmentID: new FormControl<number>(0), // Thêm ID vào form
    // Dùng nonNullable: true để tránh giá trị null nếu không cần thiết
    studentID: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required]
    }),

    // Sửa lại: Tham số đầu tiên là giá trị khởi tạo (ví dụ: null hoặc 0)
    offeringID: new FormControl<number>(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(1)] // Thêm min(1) nếu ID phải là số dương
    }),
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
    const id = this.route.snapshot.paramMap.get('id');

    // Chế độ thêm mới
    if (!id) {
      this.form.controls.studentID.valueChanges
        .pipe(
          debounceTime(500),
          distinctUntilChanged()
        )
        .subscribe(studentId => {
          if (studentId?.trim()) {
            this.loadOfferings(studentId);
          } else {
            this.offerings = [];
          }
        });

      return;
    }

    // Chế độ chỉnh sửa
    this.isEditMode.set(true);
    this.currentEnrollmentID.set(+id);

    this.service.getEnrollment(+id).subscribe(data => {
      this.form.patchValue(data);

      // Tải danh sách học phần theo đúng sinh viên
      this.loadOfferings(data.studentID);

      // Khóa các trường không cho sửa
      this.form.controls.studentID.disable();
      this.form.controls.offeringID.disable();
    });
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
