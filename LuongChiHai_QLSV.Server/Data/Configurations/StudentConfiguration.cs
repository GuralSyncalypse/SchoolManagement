using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LuongChiHai_QLSV.Server.Models;

namespace LuongChiHai_QLSV.Server.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            // 1. Định nghĩa tên bảng 
            builder.ToTable("Student");

            // 2. Cấu hình Khóa chính và các thuộc tính
            builder.HasKey(s => s.StudentID);

            builder.Property(s => s.StudentID)
                   .HasColumnType("varchar(15)") // Khớp hoàn toàn với TypeName = "varchar(15)"
                   .IsRequired();

            builder.Property(s => s.StudentName)
                   .HasColumnName("StudentName") // Chỉ định tên cột giống thuộc tính
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(s => s.Gender)
                   .HasColumnName("Gender")
                   .HasMaxLength(10)
                   .IsRequired(false); // Cho phép NULL vì trong Model bạn đặt là string?

            // =======================================================
            // 🔗 CẤU HÌNH CÁC MỐI QUAN HỆ (RELATIONSHIPS)
            // =======================================================

            // 3. Quan hệ 1 - 1: Student <-> StudentProfile
            builder.HasOne(s => s.StudentProfile)
                   .WithOne(p => p.Student)
                   .HasForeignKey<StudentProfile>(p => p.StudentID) // Khóa ngoại nằm ở bảng phụ
                   .OnDelete(DeleteBehavior.Cascade);               // Xóa Student tự động xóa Profile

            // 4. Quan hệ 1 - 1: Student <-> AcademicProfile
            builder.HasOne(s => s.AcademicProfile)
                   .WithOne(a => a.Student)
                   .HasForeignKey<AcademicProfile>(a => a.StudentID) // Khóa ngoại nằm ở bảng phụ
                   .OnDelete(DeleteBehavior.Cascade);

            // 5. Quan hệ 1 - Nhiều: Student <-> FamilyRelationship
            builder.HasMany(s => s.FamilyRelationships)
                   .WithOne(f => f.Student) // Giả định trong Model FamilyRelationship có thuộc tính: public virtual Student? Student { get; set; }
                   .HasForeignKey(f => f.StudentID)
                   .OnDelete(DeleteBehavior.Cascade);

            // 6. Quan hệ 1 - Nhiều: Student <-> Enrollment (Đăng ký học phần)
            builder.HasMany(s => s.Enrollments)
                   .WithOne(e => e.Student)
                   .HasForeignKey(e => e.StudentID)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade); // Chọn Cascade để xóa SV thì tự hủy lịch học
        }
    }
}
