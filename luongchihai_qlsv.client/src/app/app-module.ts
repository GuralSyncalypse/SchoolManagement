import { HttpClientModule } from '@angular/common/http';
import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';

import { DashboardComponent } from './dashboard/dashboard.component';
import { Students } from './students/students';
import { Courses } from './courses/courses';
import { CourseForm } from './courses/form/form';

@NgModule({
  declarations: [App],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    DashboardComponent,
    Students,
    Courses,
    CourseForm
  ],
  providers: [provideBrowserGlobalErrorListeners()],
  bootstrap: [App],
})
export class AppModule {}
