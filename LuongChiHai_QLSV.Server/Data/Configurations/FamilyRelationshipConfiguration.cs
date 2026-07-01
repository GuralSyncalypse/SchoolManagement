using LuongChiHai_QLSV.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuongChiHai_QLSV.Server.Data.Configurations
{
    public class FamilyRelationshipConfiguration : IEntityTypeConfiguration<FamilyRelationship>
    {
        public void Configure(EntityTypeBuilder<FamilyRelationship> builder)
        {
            // 1. Định nghĩa tên bảng
            builder.ToTable("FamilyRelationship");

            // 2. Cấu hình khóa chính
            builder.HasKey(e => e.FamilyMemberID);

            // 3. Cấu hình cột StudentID (Khóa ngoại)
            builder.Property(e => e.StudentID)
                   .IsRequired()
                   .HasColumnType("varchar(15)");

            // 4. Cấu hình các thông tin bắt buộc
            builder.Property(e => e.RelativeName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.RelationshipType)
                   .IsRequired()
                   .HasMaxLength(30);

            // 5. Cấu hình các thông tin cho phép Null
            builder.Property(e => e.PhoneNumber)
                   .HasMaxLength(15); // Không có IsRequired() -> cho phép null

            // 6. Cấu hình BirthYear (nếu muốn, có thể ràng buộc phạm vi nếu cần)
            builder.Property(e => e.BirthYear)
                   .IsRequired();

            // 7. Cấu hình quan hệ với bảng Student (Nhiều - Một)
            builder.HasOne(d => d.Student)
                   .WithMany(p => p.FamilyRelationships) // Đảm bảo trong class Student có thuộc tính này
                   .HasForeignKey(d => d.StudentID)
                   .OnDelete(DeleteBehavior.Cascade); // Xóa sinh viên sẽ xóa luôn danh sách người thân
        }
    }
}
