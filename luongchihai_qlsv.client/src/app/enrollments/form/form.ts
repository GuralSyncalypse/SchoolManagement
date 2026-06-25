import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EnrollmentService } from '../enrollments.service';
import { EnrollmentResponse } from '../../models';

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
    courseID: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    semester: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    processScore: new FormControl<number | null>(null),
    midtermScore: new FormControl<number | null>(null),
    finalExamScore: new FormControl<number | null>(null)
  });

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

    const rawValue = this.form.getRawValue();

    const payload = {
      ...rawValue,
      courseID: Number(rawValue.courseID) // Ép sang number ở đây
    };

    const action$ = this.isEditMode()
      ? this.service.updateEnrollment(payload.studentID, payload.courseID, payload)
      : this.service.createEnrollment(rawValue);

    action$.subscribe({
      next: () => this.router.navigate(['/enrollments']),
      error: () => this.isSubmitting.set(false)
    });
  }
}
