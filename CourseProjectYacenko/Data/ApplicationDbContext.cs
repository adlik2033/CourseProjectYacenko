using CourseProjectYacenko.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectYacenko.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация связей
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Tariffs)
                .WithOne(t => t.AppUser)
                .HasForeignKey(t => t.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Payments)
                .WithOne(p => p.AppUser)
                .HasForeignKey(p => p.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Applications)
                .WithOne(a => a.AppUser)
                .HasForeignKey(a => a.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tariff>()
                .HasMany(t => t.ConnectedServices)
                .WithOne(s => s.Tariff)
                .HasForeignKey(s => s.TariffId)
                .OnDelete(DeleteBehavior.Cascade);

            // Уникальные индексы
            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Начальные данные для тарифов
            modelBuilder.Entity<Tariff>().HasData(
                new Tariff
                {
                    Id = 1,
                    Name = "Базовый",
                    Description = "Базовый тариф для новых абонентов",
                    MonthlyFee = 300,
                    InternetTrafficGB = 5,
                    MinutesCount = 100,
                    SmsCount = 50
                },
                new Tariff
                {
                    Id = 2,
                    Name = "Стандарт",
                    Description = "Популярный тариф для ежедневного использования",
                    MonthlyFee = 500,
                    InternetTrafficGB = 15,
                    MinutesCount = 300,
                    SmsCount = 100
                },
                new Tariff
                {
                    Id = 3,
                    Name = "Премиум",
                    Description = "Тариф для активных пользователей",
                    MonthlyFee = 1000,
                    InternetTrafficGB = 30,
                    MinutesCount = 1000,
                    SmsCount = 500
                }
            );

            // Начальные данные для услуг
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
                    Name = "Международные звонки",
                    Description = "Выгодные тарифы на международные звонки",
                    Type = ServiceType.Calls,
                    Cost = 200,
                    BillingPeriod = BillingPeriod.Monthly
                }
            );
        }
    }
}