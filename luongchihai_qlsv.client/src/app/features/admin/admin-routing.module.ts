import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayoutComponent } from '../../layouts//admin-layout/admin-layout.component'
import { AdminDashboardComponent } from './dashboard/admin-dashboard.component'
import { StudentList } from './student-list/student-list'
import { FormComponent } from './student-list/form/form.component'
import { CourseList } from './course-list/course-list'

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: AdminDashboardComponent }, // URL: /admin/dashboard
      { path: 'students', component: StudentList }, // URL: /admin/student
      { path: 'students/form', component: FormComponent }, // URL: /admin/student
      { path: 'courses', component: CourseList },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' } 
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes), ReactiveFormsModule],
  exports: [RouterModule]
})

export class AdminRoutingModule { }
