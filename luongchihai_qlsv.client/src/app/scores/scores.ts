import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { Score } from '../models';
import { ScoreService } from './scores.service';

@Component({
  selector: 'app-scores',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './scores.html'
})
export class Scores implements OnInit {
  private scoreService = inject(ScoreService);
  private router = inject(Router);

  // Quản lý danh sách điểm số bằng Signal
  scores = signal<Score[]>([]);

  ngOnInit(): void {
    this.loadScores();
  }

  loadScores(): void {
    this.scoreService.getScores().subscribe({
      next: (data) => this.scores.set(data),
      error: (err) => console.error('Lỗi khi tải danh sách điểm:', err)
    });
  }

  onCreate(): void {
    this.router.navigate(['/scores/create']);
  }

  onUpdate(scoreId: number): void {
    this.router.navigate(['/scores/edit', scoreId]);
  }

  onDelete(scoreId: number): void {
    if (confirm(`Bạn muốn xóa đầu điểm #${scoreId}?`)) {
      this.scoreService.deleteScore(scoreId).subscribe({
        next: () => {
          this.scores.update(list => list.filter(s => s.scoreId !== scoreId));
        },
        error: (err) => alert('Lỗi hệ thống khi xóa điểm.')
      });
    }
  }
}
