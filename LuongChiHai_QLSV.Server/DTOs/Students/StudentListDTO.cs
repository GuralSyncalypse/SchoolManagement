namespace LuongChiHai_QLSV.Server.DTOs.Students
{
    public class StudentListDTO
    {
        public string StudentID { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        // Lấy một vài thông tin quan trọng từ bảng phụ ra ngoài để hiển thị ở bảng danh sách
        public string? ClassName { get; set; }
        public string? MajorName { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
