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

            // Связи
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Tariffs)
                .WithOne(t => t.AppUser)
                .HasForeignKey(t => t.AppUserId);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Payments)
                .WithOne(p => p.AppUser)
                .HasForeignKey(p => p.AppUserId);

            modelBuilder.Entity<Tariff>()
                .HasMany(t => t.ConnectedServices)
                .WithOne(s => s.Tariff)
                .HasForeignKey(s => s.TariffId);

            // Уникальные индексы
            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Начальные данные
            modelBuilder.Entity<Tariff>().HasData(
                new Tariff { Id = 1, Name = "Базовый", Description = "Для новых клиентов", MonthlyFee = 300, InternetTrafficGB = 5, MinutesCount = 100, SmsCount = 50 },
                new Tariff { Id = 2, Name = "Стандарт", Description = "Популярный тариф", MonthlyFee = 500, InternetTrafficGB = 15, MinutesCount = 300, SmsCount = 100 },
                new Tariff { Id = 3, Name = "Премиум", Description = "Для активных пользователей", MonthlyFee = 1000, InternetTrafficGB = 30, MinutesCount = 1000, SmsCount = 500 }
            );

            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, Name = "YouTube безлимит", Description = "Безлимитный YouTube", Type = ServiceType.Entertainment, Cost = 50, BillingPeriod = BillingPeriod.Monthly },
                new Service { Id = 2, Name = "Антивирус", Description = "Защита устройства", Type = ServiceType.Security, Cost = 100, BillingPeriod = BillingPeriod.Monthly }
            );
        }
    }
}