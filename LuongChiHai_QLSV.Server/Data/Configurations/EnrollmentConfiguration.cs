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
            builder.ToTable("Enrollment");

            // =========================
            // Primary Key
            // =========================
            builder.HasKey(e => e.EnrollmentID);

            builder.Property(e => e.EnrollmentID)
                   .ValueGeneratedOnAdd();

            // =========================
            // Unique Constraint
            // =========================
            builder.HasIndex(e => new
            {
                e.StudentID,
                e.CourseID,
                e.AcademicYear,
                e.Semester
            })
            .IsUnique();

            // =========================
            // Properties
            // =========================
            builder.Property(e => e.StudentID)
                   .IsRequired()
                   .HasMaxLength(15);

            builder.Property(e => e.CourseID)
                   .IsRequired();

            builder.Property(e => e.AcademicYear)
                   .IsRequired();

            builder.Property(e => e.Semester)
                   .IsRequired();

            builder.Property(e => e.ProcessScore)
                   .HasColumnType("decimal(4,2)");

            builder.Property(e => e.MidtermScore)
                   .HasColumnType("decimal(4,2)");

            builder.Property(e => e.FinalExamScore)
                   .HasColumnType("decimal(4,2)");

            builder.Property(e => e.WeightProcess)
                   .HasColumnType("decimal(3,2)")
                   .HasDefaultValue(0.20m);

            builder.Property(e => e.WeightMidterm)
                   .HasColumnType("decimal(3,2)")
                   .HasDefaultValue(0.30m);

            builder.Property(e => e.WeightFinal)
                   .HasColumnType("decimal(3,2)")
                   .HasDefaultValue(0.50m);

            // =========================
            // Computed Columns
            // =========================
            builder.Property(e => e.TotalScore)
                   .HasColumnType("decimal(5,2)")
                   .HasComputedColumnSql(@"
                    ISNULL(ProcessScore,0) * WeightProcess +
                    ISNULL(MidtermScore,0) * WeightMidterm +
                    ISNULL(FinalExamScore,0) * WeightFinal
               ", stored: true);

            builder.Property(e => e.IsPassed)
                   .HasComputedColumnSql(@"
                    CASE
                        WHEN (
                            ISNULL(ProcessScore,0) * WeightProcess +
                            ISNULL(MidtermScore,0) * WeightMidterm +
                            ISNULL(FinalExamScore,0) * WeightFinal
                        ) >= 4.0
                        THEN CAST(1 AS BIT)
                        ELSE CAST(0 AS BIT)
                    END
               ", stored: true);

            // =========================
            // CHECK Constraints
            // =========================
            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CHK_Enrollment_Semester",
                    "[Semester] >= 1 AND [Semester] <= 3");

                t.HasCheckConstraint(
                    "CHK_Enrollment_ProcessScore",
                    "[ProcessScore] IS NULL OR ([ProcessScore] >= 0 AND [ProcessScore] <= 10)");

                t.HasCheckConstraint(
                    "CHK_Enrollment_MidtermScore",
                    "[MidtermScore] IS NULL OR ([MidtermScore] >= 0 AND [MidtermScore] <= 10)");

                t.HasCheckConstraint(
                    "CHK_Enrollment_FinalExamScore",
                    "[FinalExamScore] IS NULL OR ([FinalExamScore] >= 0 AND [FinalExamScore] <= 10)");
            });

            // =========================
            // Relationships
            // =========================

            builder.HasOne(e => e.Student)
                   .WithMany(s => s.Enrollments)
                   .HasForeignKey(e => e.StudentID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
