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

            builder.HasKey(e => e.ScoreId);

            builder.Property(e => e.ScoreType)
                   .IsRequired()
                   .HasMaxLength(50);

            // Cấu hình DECIMAL(4,2)
            builder.Property(e => e.ScoreValue)
                   .HasColumnType("decimal(4,2)")
                   .IsRequired();

            // Cấu hình quan hệ N-1 với Enrollment
            builder.HasOne(s => s.Enrollment)
                   .WithMany(e => e.Scores)
                   .HasForeignKey(s => s.EnrollmentId)
                   .OnDelete(DeleteBehavior.Cascade); // Nếu xóa Enrollment, xóa luôn các đầu điểm
        }
    }
}
