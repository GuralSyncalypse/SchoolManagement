import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Import your components based on your folder structure
import { DashboardComponent } from './dashboard/dashboard.component';
import { Students } from './students/students';
import { Courses } from './courses/courses';
import { CourseForm } from './courses/form/form';
import { StudentForm } from './students/form/form';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'students', component: Students },
  { path: 'students/create', component: StudentForm },
  { path: 'students/edit/:id', component: StudentForm },
  { path: 'courses', component: Courses },
  { path: 'courses/create', component: CourseForm },
  { path: 'courses/edit/:id', component: CourseForm },
  { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
