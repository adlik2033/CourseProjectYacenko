using Microsoft.EntityFrameworkCore;
using MobileOperator.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CourseProjectYacenko.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация связей

            // Subscriber (1) -> Tariffs (N)
            modelBuilder.Entity<Subscriber>()
                .HasMany(s => s.Tariffs)
                .WithOne(t => t.Subscriber)
                .HasForeignKey(t => t.SubscriberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Subscriber (1) -> Payments (N)
            modelBuilder.Entity<Subscriber>()
                .HasMany(s => s.Payments)
                .WithOne(p => p.Subscriber)
                .HasForeignKey(p => p.SubscriberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Subscriber (1) -> Applications (N)
            modelBuilder.Entity<Subscriber>()
                .HasMany(s => s.Applications)
                .WithOne(a => a.Subscriber)
                .HasForeignKey(a => a.SubscriberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tariff (1) -> Services (N)
            modelBuilder.Entity<Tariff>()
                .HasMany(t => t.ConnectedServices)
                .WithOne(s => s.Tariff)
                .HasForeignKey(s => s.TariffId)
                .OnDelete(DeleteBehavior.Cascade);

            // Индексы для оптимизации
            modelBuilder.Entity<Subscriber>()
                .HasIndex(s => s.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Subscriber>()
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<Tariff>()
                .HasIndex(t => t.Name);

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.PaymentDateTime);

            modelBuilder.Entity<Application>()
                .HasIndex(a => new { a.Status, a.CreationDate });

            // Начальные данные (Seed Data)
            modelBuilder.Entity<Tariff>().HasData(
                new Tariff
                {
                    Id = 1,
                    Name = "Базовый",
                    Description = "Базовый тариф для экономных пользователей",
                    MonthlyFee = 300,
                    InternetTrafficGB = 5,
                    MinutesCount = 100,
                    SmsCount = 50
                },
                new Tariff
                {
                    Id = 2,
                    Name = "Стандарт",
                    Description = "Популярный тариф для повседневного использования",
                    MonthlyFee = 500,
                    InternetTrafficGB = 15,
                    MinutesCount = 300,
                    SmsCount = 100
                },
                new Tariff
                {
                    Id = 3,
                    Name = "Премиум",
                    Description = "Тариф для активных пользователей с безлимитными возможностями",
                    MonthlyFee = 1000,
                    InternetTrafficGB = 50,
                    MinutesCount = 1000,
                    SmsCount = 500
                }
            );

            modelBuilder.Entity<Service>().HasData(
                new Service
                {
                    Id = 1,
                    Name = "Безлимитный YouTube",
                    Description = "Просмотр YouTube без расхода трафика",
                    Type = ServiceType.Entertainment,
                    Cost = 50,
                    BillingPeriod = BillingPeriod.Monthly
                },
                new Service
                {
                    Id = 2,
                    Name = "Антивирус",
                    Description = "Защита устройства от вирусов",
                    Type = ServiceType.Security,
                    Cost = 100,
                    BillingPeriod = BillingPeriod.Monthly
                },
                new Service
                {
                    Id = 3,
                    Name = "Роуминг по России",
                    Description = "Выгодные тарифы в поездках по России",
                    Type = ServiceType.Calls,
                    Cost = 200,
                    BillingPeriod = BillingPeriod.Monthly
                }
            );
        }
    }
}