using Microsoft.EntityFrameworkCore;

namespace LuongChiHai_QLSV.Server.Models
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        // EF Core sẽ tự động ánh xạ (Map) các thuộc tính này thành các bảng trong SQL
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<Score> Scores { get; set; } = null!;
    }
}
