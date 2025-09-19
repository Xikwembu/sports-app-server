using Microsoft.EntityFrameworkCore;
using Sport_App_Model.Entity;
using Sports_App_Model.Entity;

namespace Sport_App_Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LoginOtp> LoginOtps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.LoginOtp)
                .WithOne(o => o.User)
                .HasForeignKey<LoginOtp>(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
