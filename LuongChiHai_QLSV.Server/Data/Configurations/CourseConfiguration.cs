using LuongChiHai_QLSV.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuongChiHai_QLSV.Server.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // 1. Table mapping
            builder.ToTable("Course");

            // 2. Primary key
            builder.HasKey(e => e.CourseID);

            // 3. Properties
            builder.Property(e => e.CourseName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.CourseCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Credits)
                .IsRequired(false);

            // 4. Index (unique course code)
            builder.HasIndex(e => e.CourseCode)
                .IsUnique();

            // 5. Relationship: Course (1) -> (N) CourseOffering
            builder.HasMany(e => e.CourseOfferings)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
