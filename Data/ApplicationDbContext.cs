using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniSocialMediaApp.Models;

namespace MiniSocialMediaApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Post> Posts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasOne<IdentityUser>()
                      .WithMany()
                      .HasForeignKey(p => p.AuthorId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.Content)
                      .HasMaxLength(140)
                      .IsRequired();

                entity.Property(p => p.Title)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.Property(p => p.CreatedAt)
                      .HasConversion(
                          v => v.ToString("yyyy-MM-dd HH:mm:ss"),
                          v => DateTimeOffset.Parse(v))
                      .IsRequired();

                entity.Property(p => p.EditedAt)
                      .HasConversion(
                          v => v.HasValue ? v.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                          v => v != null ? DateTimeOffset.Parse(v) : (DateTimeOffset?)null);
            });
        }
    }
}