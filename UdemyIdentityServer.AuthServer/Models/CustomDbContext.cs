using Microsoft.EntityFrameworkCore;

namespace UdemyIdentityServer.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CustomUser> CustomUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser()
                {
                    Id = 1,
                    Email = "caglasavas@gmail.com",
                    Password = "Password12*",
                    City = "İstanbul",
                    UserName = "caglasavas"
                },
                new CustomUser()
                {
                    Id = 2,
                    Email = "ercansavas@gmail.com",
                    Password = "Password12*",
                    City = "Ankara",
                    UserName = "ercansavas"
                },
                new CustomUser()
                {
                    Id = 3,
                    Email = "mehmet@gmail.com",
                    Password = "Password12*",
                    City = "Konya",
                    UserName = "mehmets"
                }
             );

            base.OnModelCreating(modelBuilder);
        }
    }
}
