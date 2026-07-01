using LuongChiHai_QLSV.Server.Data.Configurations;
using LuongChiHai_QLSV.Server.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LuongChiHai_QLSV.Server.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<AcademicProfile> AcademicProfiles { get; set; } = null!;
        public DbSet<FamilyRelationship> FamilyRelationships { get; set; } = null!;

        // Các bảng cũ của bạn
        public DbSet<Course> Courses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Áp dụng các cấu hình cho việc quản lý thông tin sinh viên
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new AcademicProfileConfiguration());
            modelBuilder.ApplyConfiguration(new FamilyRelationshipConfiguration());

            // Áp dụng các cấu hình cho việc quản lý môn học
            modelBuilder.ApplyConfiguration(new CourseConfiguration());

            // 1. Cấu hình Khóa phức hợp cho UserRole (UserID, RoleID)
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserID, ur.RoleID });

            // 2. Đảm bảo luật Unique cho Username
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // 3. Đảm bảo luật Unique cho RoleName
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            // Tự động tìm tất cả các file có kế thừa IEntityTypeConfiguration trong toàn bộ Project và nạp vào.
            // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
