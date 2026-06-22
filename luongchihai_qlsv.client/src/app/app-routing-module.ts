import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Import your components based on your folder structure
import { DashboardComponent } from './dashboard/dashboard.component';
import { Students } from './students/students';
// import { CoursesComponent } from './courses/courses.component';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'students', component: Students },
  { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
