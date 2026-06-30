import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StudentListComponent } from './pages/student-list/student-list.component';

const routes: Routes = [
  { path: '', component: StudentListComponent }, // /students
];

@NgModule({
  imports: [RouterModule.forChild(routes)], // Dùng forChild cho module con
  exports: [RouterModule]
})
export class StudentsRoutingModule { }
