import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { DashboardComponent } from './dashboard/dashboard.component';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';

@NgModule({
  declarations: [
    App,
    LoginComponent,
    RegisterComponent, // Đăng ký trang Login tại đây
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    DashboardComponent, // Giữ nguyên nếu nó là Standalone component
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    // ĐĂNG KÝ INTERCEPTOR TẠI ĐÂY 👇
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true,
    },
  ],
  bootstrap: [App],
})
export class AppModule {}
