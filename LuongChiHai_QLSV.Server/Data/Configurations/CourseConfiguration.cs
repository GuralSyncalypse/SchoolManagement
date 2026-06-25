using LuongChiHai_QLSV.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuongChiHai_QLSV.Server.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // 1. Cấu hình bảng
            builder.ToTable("Course");

            // 2. Cấu hình khóa chính
            builder.HasKey(e => e.CourseID);

            // 3. Cấu hình các thuộc tính
            builder.Property(e => e.CourseName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.CourseCode)
                   .IsRequired()
                   .HasMaxLength(100);

            // 4. Tạo Index cho CourseCode vì thường dùng để tìm kiếm nhanh
            builder.HasIndex(e => e.CourseCode)
                   .IsUnique();

            // 5. Cấu hình Credits
            builder.Property(e => e.Credits)
                   .IsRequired(false); // Cho phép Null
        }
    }
}
