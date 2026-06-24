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
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<Score> Scores { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tự động tìm tất cả các file có kế thừa IEntityTypeConfiguration trong toàn bộ Project và nạp vào.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
