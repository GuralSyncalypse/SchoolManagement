import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EnrollmentForm } from './form';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';

describe('EnrollmentForm', () => {
  let component: EnrollmentForm;
  let fixture: ComponentFixture<EnrollmentForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EnrollmentForm],
      providers: [provideHttpClient(), provideRouter([])]
    }).compileComponents();

    fixture = TestBed.createComponent(EnrollmentForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
