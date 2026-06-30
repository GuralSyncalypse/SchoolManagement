import { Component, inject } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../core/services/auth.service'; // 👈 Nhớ sửa lại đường dẫn cho đúng file của bạn

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  // Inject các dịch vụ cần thiết
  private authService = inject(AuthService);
  private router = inject(Router);

  // Hàm xử lý khi bấm nút Đăng xuất
  onLogout(): void {
    if (confirm('Bạn có chắc chắn muốn đăng xuất không?')) {
      this.authService.logout();        // Gọi hàm xóa token của bạn
      this.router.navigate(['/login']); // Điều hướng admin về trang đăng nhập của ASP.NET Core / Angular
    }
  }
}
