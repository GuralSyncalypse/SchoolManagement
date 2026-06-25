import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Score } from '../models';

@Injectable({ providedIn: 'root' })
export class ScoreService {
  private http = inject(HttpClient);
  private apiUrl = '/api/scores';

  // Lấy toàn bộ danh sách điểm
  getScores(): Observable<Score[]> {
    return this.http.get<Score[]>(this.apiUrl);
  }

  // Lấy chi tiết điểm theo ID (vẫn giữ nguyên nếu API server dùng ScoreId làm khóa chính)
  getScoreById(scoreId: number): Observable<Score> {
    return this.http.get<Score>(`${this.apiUrl}/${scoreId}`);
  }

  // Thêm điểm mới
  createScore(score: Score): Observable<Score> {
    return this.http.post<Score>(this.apiUrl, score);
  }

  // Cập nhật điểm theo ScoreId
  updateScore(scoreId: number, score: Score): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${scoreId}`, score);
  }

  // Xóa điểm theo ScoreId
  deleteScore(scoreId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${scoreId}`);
  }
}
