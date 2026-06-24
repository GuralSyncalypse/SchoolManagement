using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LuongChiHai_QLSV.Server.Models;

namespace LuongChiHai_QLSV.Server.Data.Configurations
{
    public class StudentProfileConfiguration : IEntityTypeConfiguration<StudentProfile>
    {
        public void Configure(EntityTypeBuilder<StudentProfile> builder)
        {
            // 1. Định nghĩa tên bảng
            builder.ToTable("StudentProfile");

            // 2. Cấu hình Khóa chính và kiểu dữ liệu đặc biệt
            builder.HasKey(e => e.StudentID);
            builder.Property(e => e.StudentID)
                  .HasColumnType("varchar(15)");

            // 3. Cấu hình các thông tin cơ bản (BẮT BUỘC)
            builder.Property(e => e.BirthDate);

            builder.Property(e => e.PhoneNumber)
                  .IsRequired()
                  .HasMaxLength(15)
                  .HasColumnType("varchar(15)");

            builder.Property(e => e.Email)
                  .IsRequired()
                  .HasMaxLength(100)
                  .HasColumnType("varchar(100)");

            // 4. Cấu hình các thông tin bổ sung sau (CHO PHÉP NULL + ĐỘ DÀI)
            builder.Property(e => e.Ethnicity).HasMaxLength(30);
            builder.Property(e => e.Religion).HasMaxLength(50);
            builder.Property(e => e.Nationality).HasMaxLength(50);
            builder.Property(e => e.BirthPlace).HasMaxLength(100);

            builder.Property(e => e.CitizenID)
                  .HasMaxLength(12)
                  .HasColumnType("char(12)");

            builder.Property(e => e.CitizenIDIssuePlace).HasMaxLength(100);
            builder.Property(e => e.PermanentAddress).HasMaxLength(255);
            builder.Property(e => e.TemporaryAddress).HasMaxLength(255);

            // 5. CẤU HÌNH QUAN HỆ 1 - 1 VỚI BẢNG STUDENT VÀ CASCADE DELETE
            builder.HasOne(d => d.Student)
                  .WithOne(p => p.StudentProfile)
                  .HasForeignKey<StudentProfile>(d => d.StudentID)
                  .OnDelete(DeleteBehavior.Cascade);

            // 6. TẠO FILTERED INDEX CHO CITIZENID (GIẢI PHÁP CHẶN TRÙNG CCCD KHI NULL)
            builder.HasIndex(e => e.CitizenID)
                  .IsUnique()
                  .HasFilter("[CitizenID] IS NOT NULL");

            // 7. Tạo unique cho Email và SĐT
            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(e => e.PhoneNumber).IsUnique();
        }
    }
}
