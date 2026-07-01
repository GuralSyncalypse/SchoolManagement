import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayoutComponent } from '../../layouts//admin-layout/admin-layout.component'
import { AdminDashboardComponent } from './dashboard/admin-dashboard.component'
import { StudentList } from './student-list/student-list'

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: AdminDashboardComponent }, // URL: /admin/dashboard
      { path: 'students', component: StudentList }, // URL: /admin/student
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' } 
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)], // Dùng forChild cho module con
  exports: [RouterModule]
})

export class AdminRoutingModule { }
