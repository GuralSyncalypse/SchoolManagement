import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { authGuard } from './core/guards/auth.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },

  // Các trang dùng chung cần đăng nhập mới vào được
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard], data: { expectedRoles: ['Admin', 'Student'] } },
  { path: 'register', component: RegisterComponent },
  // PHÂN HỆ ADMIN: Chỉ Admin mới được load module quản lý
  {
    path: 'students',
    canActivate: [authGuard],
    data: { expectedRoles: ['Admin'] },
    loadChildren: () => import('./students/students-routing.module').then(m => m.StudentsRoutingModule)
  },
  // {
  //   path: 'courses',
  //   canActivate: [authGuard],
  //   data: { expectedRoles: ['Admin'] },
  //   loadChildren: () => import('./courses/courses.module').then(m => m.CoursesModule)
  // },

  // PHÂN HỆ STUDENT: Trang xem kết quả riêng cho sinh viên
  // {
  //   path: 'my-results',
  //   canActivate: [authGuard],
  //   data: { expectedRoles: ['Student'] },
  //   loadChildren: () => import('./student-results/student-results.module').then(m => m.StudentResultsModule)
  // },

  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
