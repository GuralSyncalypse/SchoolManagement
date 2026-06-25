using LuongChiHai_QLSV.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuongChiHai_QLSV.Server.Data.Configurations
{
    public class ScoreConfiguration : IEntityTypeConfiguration<Score>
    {
        public void Configure(EntityTypeBuilder<Score> builder)
        {
            builder.ToTable("Score");

            // 1. Khóa chính
            builder.HasKey(s => s.ScoreId);

            // 2. Cấu hình các thuộc tính
            builder.Property(s => s.ScoreType)
                  .HasMaxLength(50)
                  .IsRequired();

            // DECIMAL(4,2) trong C# dùng kiểu decimal
            builder.Property(s => s.ScoreValue)
                  .HasColumnType("decimal(4,2)")
                  .IsRequired();

            // 3. Cấu hình liên kết với Enrollment (Khóa ngoại kép)
            builder.HasOne(s => s.Enrollment)
                  .WithMany(e => e.Scores)
                  // Chỉ định rõ cặp khóa ngoại tham chiếu đến (StudentID, CourseID)
                  .HasForeignKey(s => new { s.StudentID, s.CourseID })
                  .OnDelete(DeleteBehavior.Cascade); // Xóa Enrollment sẽ xóa toàn bộ điểm liên quan
        }
    }
}
