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

            // Настройка точности для decimal свойств
            modelBuilder.Entity<AppUser>()
                .Property(u => u.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Tariff>()
                .Property(t => t.MonthlyFee)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Service>()
                .Property(s => s.Cost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            // Настройка nullable свойств
            modelBuilder.Entity<AppUser>()
                .Property(u => u.RefreshToken)
                .IsRequired(false);

            modelBuilder.Entity<AppUser>()
                .Property(u => u.RefreshTokenExpiryTime)
                .IsRequired(false);

            modelBuilder.Entity<AppUser>()
                .Property(u => u.LastLoginDate)
                .IsRequired(false);

            modelBuilder.Entity<AppUser>()
                .Property(u => u.Address)
                .IsRequired(false);

            modelBuilder.Entity<AppUser>()
                .Property(u => u.PassportData)
                .IsRequired(false);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Tariffs)
                .WithMany(t => t.Users);

            modelBuilder.Entity<Tariff>()
                .HasMany(t => t.ConnectedServices)
                .WithOne(s => s.Tariff)
                .HasForeignKey(s => s.TariffId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Tariff>().HasData(
                new Tariff
                {
                    Id = 1,
                    Name = "Базовый",
                    Description = "Для новых клиентов",
                    MonthlyFee = 300.00m,
                    InternetTrafficGB = 5,
                    MinutesCount = 100,
                    SmsCount = 50
                },
                new Tariff
                {
                    Id = 2,
                    Name = "Стандарт",
                    Description = "Популярный тариф",
                    MonthlyFee = 500.00m,
                    InternetTrafficGB = 15,
                    MinutesCount = 300,
                    SmsCount = 100
                },
                new Tariff
                {
                    Id = 3,
                    Name = "Премиум",
                    Description = "Для активных пользователей",
                    MonthlyFee = 1000.00m,
                    InternetTrafficGB = 30,
                    MinutesCount = 1000,
                    SmsCount = 500
                }
            );

            // Услуги (без привязки к тарифам при инициализации)
            modelBuilder.Entity<Service>().HasData(
                new Service
                {
                    Id = 1,
                    Name = "YouTube безлимит",
                    Description = "Безлимитный YouTube",
                    Type = ServiceType.Entertainment,
                    Cost = 50.00m,
                    BillingPeriod = BillingPeriod.Monthly
                },
                new Service
                {
                    Id = 2,
                    Name = "Антивирус",
                    Description = "Защита устройства",
                    Type = ServiceType.Security,
                    Cost = 100.00m,
                    BillingPeriod = BillingPeriod.Monthly
                },
                new Service
                {
                    Id = 3,
                    Name = "Музыка",
                    Description = "Безлимитная музыка",
                    Type = ServiceType.Entertainment,
                    Cost = 30.00m,
                    BillingPeriod = BillingPeriod.Monthly
                }
            );

            // Администратор
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    FullName = "Администратор Системы",
                    PhoneNumber = "+79998887766",
                    Email = "admin@mobileoperator.ru",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Address = "г. Москва, ул. Административная, д. 1",
                    PassportData = "0000 000000",
                    Balance = 10000.00m,
                    Role = "Admin",
                    IsActive = true,
                    RegistrationDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}