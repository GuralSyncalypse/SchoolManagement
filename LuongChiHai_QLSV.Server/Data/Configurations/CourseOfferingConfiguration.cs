using LuongChiHai_QLSV.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuongChiHai_QLSV.Server.Data.Configurations
{
    public class CourseOfferingConfiguration : IEntityTypeConfiguration<CourseOffering>
    {
        public void Configure(EntityTypeBuilder<CourseOffering> builder)
        {
            // 1. Table
            builder.ToTable("CourseOffering");

            // 2. Primary key
            builder.HasKey(e => e.OfferingID);

            // 3. Properties

            builder.Property(e => e.CourseID)
                .IsRequired();

            builder.Property(e => e.YearID)
                .IsRequired();

            builder.Property(e => e.TypeID)
                .IsRequired();

            builder.Property(e => e.InstructorID)
                .IsRequired(false);

            builder.Property(e => e.MaxStudents)
                .IsRequired(false);

            builder.Property(e => e.CurrentStudents)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(e => e.IsOpen)
                .HasDefaultValue(true);

            // 4. Unique constraint: 1 môn chỉ mở 1 lần / năm / kỳ
            builder.HasIndex(e => new { e.CourseID, e.YearID, e.TypeID })
                .IsUnique();

            // 5. Relationships

            builder.HasOne(e => e.Course)
                .WithMany(c => c.CourseOfferings)
                .HasForeignKey(e => e.CourseID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.AcademicYear)
                .WithMany()
                .HasForeignKey(e => e.YearID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.SemesterType)
                .WithMany()
                .HasForeignKey(e => e.TypeID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
