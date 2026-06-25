import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Score } from '../../models';
import { ScoreService } from '../scores.service';

@Component({
  selector: 'app-score-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './form.html'
})
export class ScoreForm implements OnInit {
  private scoreService = inject(ScoreService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  isSubmitting = signal<boolean>(false);
  isEditMode = signal<boolean>(false);

  // Khởi tạo form với các trường mới theo cấu trúc khóa ngoại kép
  scoreForm = new FormGroup({
    scoreId: new FormControl<number>(0),
    studentID: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.maxLength(15)]
    }),
    courseID: new FormControl<number>(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(1)]
    }),
    scoreType: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(2)]
    }),
    scoreValue: new FormControl<number>(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(0), Validators.max(10)]
    })
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      const targetId = parseInt(idParam, 10);
      this.isEditMode.set(true);

      this.scoreService.getScoreById(targetId).subscribe({
        next: (data) => {
          this.scoreForm.patchValue({
            scoreId: data.scoreId,
            studentID: data.studentID,
            courseID: data.courseID,
            scoreType: data.scoreType,
            scoreValue: data.scoreValue
          });
        },
        error: () => alert('Không thể tải thông tin điểm.')
      });
    }
  }

  onSubmit(): void {
    if (this.scoreForm.invalid) {
      this.scoreForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    const raw = this.scoreForm.getRawValue();

    // Tạo đối tượng Score đồng bộ với Model mới
    const scoreData: Score = {
      scoreId: raw.scoreId ?? 0,
      studentID: raw.studentID,
      courseID: raw.courseID,
      scoreType: raw.scoreType,
      scoreValue: raw.scoreValue
    };

    if (this.isEditMode()) {
      this.scoreService.updateScore(scoreData.scoreId, scoreData).subscribe({
        next: () => {
          alert('Cập nhật điểm thành công.');
          this.router.navigate(['/scores']);
        },
        error: (err) => this.handleError(err)
      });
    } else {
      this.scoreService.createScore(scoreData).subscribe({
        next: () => {
          alert('Nhập điểm thành công.');
          this.router.navigate(['/scores']);
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
