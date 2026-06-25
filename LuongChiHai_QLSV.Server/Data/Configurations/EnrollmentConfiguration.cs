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

            // 1. Định nghĩa Composite Key
            builder.HasKey(e => new { e.StudentID, e.CourseID });

            // 2. Cấu hình các thuộc tính
            builder.Property(e => e.StudentID)
                  .HasMaxLength(15)
                  .IsRequired();

            builder.Property(e => e.EnrollmentDate)
                  .HasDefaultValueSql("GETDATE()");

            // 3. Quan hệ với Student và Course (Nếu chưa cấu hình ở các entity đó)
            builder.HasOne(e => e.Student)
                  .WithMany(s => s.Enrollments)
                  .HasForeignKey(e => e.StudentID);

            builder.HasOne(e => e.Course)
                  .WithMany(c => c.Enrollments)
                  .HasForeignKey(e => e.CourseID);

            // 4. Liên kết 1-nhiều với Score
            // Vì Enrollment có khóa chính kép, Score cần ngoại khóa tương ứng
            builder.HasMany(e => e.Scores)
                  .WithOne(s => s.Enrollment)
                  .HasForeignKey(s => new { s.StudentID, s.CourseID })
                  .OnDelete(DeleteBehavior.Cascade); // Xóa Enrollment sẽ xóa hết các điểm liên quan
        }
    }
}
