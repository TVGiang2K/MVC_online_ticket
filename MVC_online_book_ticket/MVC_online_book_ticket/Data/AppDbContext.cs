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
                    Name = "Quản trị viên",
                    Age = 30,
                    Phone = "123456789",
                    Gender = true,
                    Avatar = "admin-avatar-url",
                    Qualification = "Chứng chỉ Quản trị viên",
                    Position = "Vị trí Quản trị viên",
                    Role = Role.Administrator,
                }
            );

           

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
        public DbSet<Financial> Financials { get; set; }
        public DbSet<Reservations> Reservations { get; set; }


    }
}
