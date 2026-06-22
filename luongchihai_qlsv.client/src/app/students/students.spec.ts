import '@angular/compiler'; // Đảm bảo có JIT compiler
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing'; // <-- THÊM DÒNG NÀY
import { Students } from './students';

describe('Students', () => {
  let component: Students;
  let fixture: ComponentFixture<Students>;
  let httpMock: HttpTestingController; // <-- KHAI BÁO BIẾN BIẾN MOCK

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      // Nạp HttpClientTestingModule vào mảng imports vì Students là Standalone Component
      imports: [HttpClientTestingModule, Students],
    }).compileComponents();

    fixture = TestBed.createComponent(Students);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController); // <-- ĐẶT ĐƯỜNG TRUYỀN MOCK

    // Kích hoạt thay đổi (ngOnInit sẽ được gọi tại đây và sinh ra request /api/students)
    fixture.detectChanges();
  });

  afterEach(() => {
    httpMock.verify(); // Đảm bảo xử lý hết tất cả các request mạng giả lập
  });

  it('should create', () => {
    expect(component).toBeTruthy();

    // BẮT CHẶN REQUEST BỊ LỖI Ở TRÊN:
    // Vì ngOnInit tự động gọi API, ta phải dùng httpMock để giải cứu nó ngay lập tức
    const req = httpMock.expectOne('/api/students');
    expect(req.request.method).toEqual('GET');

    // Trả về mảng sinh viên giả lập (Dữ liệu trống hoặc tùy bạn) để dập tắt lỗi
    req.flush([]);
  });
});
