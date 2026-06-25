using LuongChiHai_QLSV.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuongChiHai_QLSV.Server.Data.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.ToTable("Enrollment");

            builder.HasKey(e => e.EnrollmentId);

            builder.Property(e => e.StudentID)
                   .IsRequired()
                   .HasColumnType("varchar(15)");

            builder.Property(e => e.EnrollmentDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()"); // Mặc định là ngày giờ hiện tại

            // Cấu hình quan hệ N-1 với Student
            builder.HasOne(e => e.Student)
                   .WithMany(s => s.Enrollments)
                   .HasForeignKey(e => e.StudentID)
                   .OnDelete(DeleteBehavior.Restrict); // Tránh xóa Student nếu vẫn còn Enrollment

            // Cấu hình quan hệ N-1 với Course
            builder.HasOne(e => e.Course)
                   .WithMany(c => c.Enrollments)
                   .HasForeignKey(e => e.CourseID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
