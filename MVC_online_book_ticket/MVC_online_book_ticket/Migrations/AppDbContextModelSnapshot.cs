﻿// <auto-generated />
using System;
using MVC_online_book_ticket.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MVC_online_book_ticket.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MVC_online_book_ticket.Models.Accounts", b =>
                {
                    b.Property<int>("AccountsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountsId"), 1L, 1);

                    b.Property<byte>("Age")
                        .HasColumnType("tinyint");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Create_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Delete_date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Qualification")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<byte>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)1);

                    b.Property<DateTime?>("Update_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AccountsId");

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            AccountsId = 1,
                            Age = (byte)30,
                            Avatar = "admin-avatar-url",
                            Create_date = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = true,
                            Name = "Quản trị viên",
                            Password = "oSBAHbpB4kn7X7QOcrBtXDaUaFtSU+/ymFHV1PkWNtOQOhTyqwPDoizoJyKlT5W5",
                            Phone = "123456789",
                            Position = "Vị trí Quản trị viên",
                            Qualification = "Chứng chỉ Quản trị viên",
                            Role = 0,
                            Status = (byte)0,
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("MVC_online_book_ticket.Models.Buses", b =>
                {
                    b.Property<int>("BusesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BusesId"), 1L, 1);

                    b.Property<string>("BusNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BusTypes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Create_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Delete_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeatCapacity")
                        .HasColumnType("int");

                    b.Property<byte>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)1);

                    b.Property<DateTime?>("Update_date")
                        .HasColumnType("datetime2");

                    b.HasKey("BusesId");

                    b.HasIndex("BusNumber")
                        .IsUnique();

                    b.ToTable("Buses");
                });

            modelBuilder.Entity("MVC_online_book_ticket.Models.Customers", b =>
                {
                    b.Property<int>("CustomersId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomersId"), 1L, 1);

                    b.Property<byte>("Age")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("Create_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Delete_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)1);

                    b.Property<DateTime?>("Update_date")
                        .HasColumnType("datetime2");

                    b.HasKey("CustomersId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("MVC_online_book_ticket.Models.Financial", b =>
                {
                    b.Property<int>("FinancialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FinancialId"), 1L, 1);

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<DateTime>("Create_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Delete_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("FinancialFrom")
                        .HasColumnType("int");

                    b.Property<int>("FinancialTo")
                        .HasColumnType("int");

                    b.Property<double>("Percentage")
                        .HasColumnType("float");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Update_date")
                        .HasColumnType("datetime2");

                    b.HasKey("FinancialId");

                    b.ToTable("Financials");
                });

            modelBuilder.Entity("MVC_online_book_ticket.Models.Reservations", b =>
                {
                    b.Property<int>("ReservationsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservationsId"), 1L, 1);

                    b.Property<string>("AccountsId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AccountsId1")
                        .HasColumnType("int");

                    b.Property<byte>("ActiveReservation")
                        .HasColumnType("tinyint");

                    b.Property<DateTime?>("CancellationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Create_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomersId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Delete_date")
                        .HasColumnType("datetime2");

                    b.Property<double?>("RefundAmount")
                        .HasColumnType("float");

                    b.Property<DateTime>("ReservationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<int>("TripsId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Update_date")
                        .HasColumnType("datetime2");

                    b.HasKey("ReservationsId");

                    b.HasIndex("AccountsId1");

                    b.HasIndex("CustomersId");

                    b.HasIndex("TripsId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("MVC_online_book_ticket.Models.Trips", b =>
                {
                    b.Property<int>("TripsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TripsId"), 1L, 1);

                    b.Property<double>("BasePrice")
                        .HasColumnType("float");

                    b.Property<int>("BusesId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Create_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Delete_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DepartureDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartureLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DestinationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DestinationLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Distance")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RouteTrip")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)1);

                    b.Property<DateTime?>("Update_date")
                        .HasColumnType("datetime2");

                    b.HasKey("TripsId");

                    b.HasIndex("BusesId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("MVC_online_book_ticket.Models.Reservations", b =>
                {
                    b.HasOne("MVC_online_book_ticket.Models.Accounts", "Accounts")
                        .WithMany()
                        .HasForeignKey("AccountsId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVC_online_book_ticket.Models.Customers", "Customers")
                        .WithMany()
                        .HasForeignKey("CustomersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVC_online_book_ticket.Models.Trips", "Buses")
                        .WithMany()
                        .HasForeignKey("TripsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accounts");

                    b.Navigation("Buses");

                    b.Navigation("Customers");
                });

            modelBuilder.Entity("MVC_online_book_ticket.Models.Trips", b =>
                {
                    b.HasOne("MVC_online_book_ticket.Models.Buses", "Buses")
                        .WithMany()
                        .HasForeignKey("BusesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Buses");
                });
#pragma warning restore 612, 618
        }
    }
}
