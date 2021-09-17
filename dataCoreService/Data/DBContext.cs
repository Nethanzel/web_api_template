using dataCoreService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace dataCoreService.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        //public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                FirstName = "User One",
                LastName = "First",
                Username = "FirstUser",
                Password = "password1"
            }, new User
            {
                Id = 2,
                FirstName = "User Two",
                LastName = "Second",
                Username = "SecondUser",
                Password = "password2"
            });
        }
    }
}