using LuongChiHai_QLSV.Server.Data.Configurations;
using LuongChiHai_QLSV.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LuongChiHai_QLSV.Server.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<AcademicProfile> AcademicProfiles { get; set; } = null!;
        public DbSet<StudentProfile> StudentProfiles { get; set; } = null!;
        public DbSet<FamilyRelationship> FamilyRelationships { get; set; } = null!;

        // Các bảng cũ của bạn
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<AcademicYear> AcademicYears { get; set; } = null!;
        public DbSet<SemesterType> SemesterTypes { get; set; } = null!;
        public DbSet<CourseOffering> CourseOfferings { get; set; } = null!;

        public DbSet<Enrollment> Enrollments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Áp dụng các cấu hình cho việc quản lý thông tin sinh viên
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new StudentProfileConfiguration());
            modelBuilder.ApplyConfiguration(new AcademicProfileConfiguration());
            modelBuilder.ApplyConfiguration(new FamilyRelationshipConfiguration());

            // Áp dụng các cấu hình cho việc quản lý môn học
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new CourseOfferingConfiguration());

            // Áp dụng các cấu hình cho việc đăng ký môn
            modelBuilder.ApplyConfiguration(new EnrollmentConfiguration());

            // Tự động tìm tất cả các file có kế thừa IEntityTypeConfiguration trong toàn bộ Project và nạp vào.
            // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
