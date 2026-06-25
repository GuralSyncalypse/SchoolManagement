using LuongChiHai_QLSV.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace LuongChiHai_QLSV.Server.Data.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            // 1. Tên bảng
            builder.ToTable("Enrollment");

            // 2. Khóa chính phức hợp
            builder.HasKey(e => new { e.StudentID, e.CourseID });

            // 3. Cấu hình thuộc tính
            builder.Property(e => e.Semester).IsRequired().HasMaxLength(20);
            builder.Property(e => e.EnrollmentDate).HasDefaultValueSql("GETDATE()");

            // Decimal precision
            builder.Property(e => e.ProcessScore).HasColumnType("decimal(4,2)");
            builder.Property(e => e.MidtermScore).HasColumnType("decimal(4,2)");
            builder.Property(e => e.FinalExamScore).HasColumnType("decimal(4,2)");
            builder.Property(e => e.TotalScore).HasColumnType("decimal(4,2)");

            builder.Property(e => e.WeightProcess).HasColumnType("decimal(3,2)").HasDefaultValue(0.20m);
            builder.Property(e => e.WeightMidterm).HasColumnType("decimal(3,2)").HasDefaultValue(0.30m);
            builder.Property(e => e.WeightFinal).HasColumnType("decimal(3,2)").HasDefaultValue(0.50m);

            // 4. Quan hệ (Định nghĩa duy nhất một lần)
            // Cấu hình liên kết với Student
            builder.HasOne(e => e.Student)
                   .WithMany(s => s.Enrollments) // <-- BẮT BUỘC TRỎ ĐÚNG VÀO COLLECTION
                   .HasForeignKey(e => e.StudentID)
                   .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình liên kết với Course
            builder.HasOne(e => e.Course)
                   .WithMany(c => c.Enrollments) // <-- BẮT BUỘC TRỎ ĐÚNG VÀO COLLECTION
                   .HasForeignKey(e => e.CourseID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(t => t.HasTrigger("trg_AutoCalculateScore"));
        }
    }
}
