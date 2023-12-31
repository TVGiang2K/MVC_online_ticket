using Microsoft.EntityFrameworkCore;
using MVC_online_book_ticket.Common;
using MVC_online_book_ticket.Models;

namespace MVC_online_book_ticket.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            var pass = new HashPassword();
            var adminUser = modelBuilder.Entity<Accounts>().HasData(
                new Accounts
                {
                    AccountsId = 1,
                    Username = "admin",
                    Password = pass.SetPassword("1234$"),
                    Email = "SRCTravelAgencies@gmail.com",
                    Name = "Administrator",
                    Age = 30,
                    Birthday = new DateTime(2024, 1, 1),
                    Phone = "0333333333",
                    Gender = true,
                    Avatar = "user-1.jpg",
                    Qualification = "Administrator Certificate",
                    Position = "Administrator position",
                    Role = Role.Administrator,
                }
            );


            modelBuilder.Entity<Accounts>()
            .HasIndex(e => e.Email)
            .IsUnique();

            modelBuilder.Entity<Accounts>()
            .HasIndex(e => e.Phone)
            .IsUnique();

            modelBuilder.Entity<Accounts>()
           .HasIndex(e => e.Username)
           .IsUnique();


            modelBuilder.Entity<Buses>()
            .HasIndex(e => e.BusNumber)
            .IsUnique();

        }

        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Buses> Buses { get; set; }
        public DbSet<Trips> Trips { get; set; }
        public DbSet<Financials> Financials { get; set; }
        public DbSet<Reservations> Reservations { get; set; }


    }
}
