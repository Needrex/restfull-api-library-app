using Microsoft.EntityFrameworkCore;
using RestApiApp.Models;

namespace RestApiApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<UserOtps> UserOtps { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>()
                .HasOne(u => u.User)
                .WithOne(r => r.RefreshToken)
                .HasForeignKey<RefreshToken>(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Books>()
                .HasOne(b => b.UserCreate)
                .WithMany(u => u.CreatedBooks)
                .HasForeignKey(b => b.UserCreateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Books>()
                .HasOne(b => b.UserUpdate)
                .WithMany(u => u.UpdatedBooks)
                .HasForeignKey(b => b.UserUpdateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserOtps>()
                .HasOne(o => o.Users)
                .WithMany(u => u.UserOtps)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(s => s.Users)
                .WithMany(u => u.ShoppingCart)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade); 
            
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(s => s.Books)
                .WithMany(b => b.ShoppingCart)
                .HasForeignKey(s => s.BooksId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}