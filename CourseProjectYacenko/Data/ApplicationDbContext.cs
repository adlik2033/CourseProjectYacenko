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
                .IsRequired(false); // Явно указываем, что это поле может быть null

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

            // Дополнительные настройки
            modelBuilder.Entity<AppUser>()
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<AppUser>()
                .Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<AppUser>()
                .Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);

            modelBuilder.Entity<AppUser>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<AppUser>()
                .Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Tariff>()
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Tariff>()
                .Property(t => t.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Service>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Service>()
                .Property(s => s.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Application>()
                .Property(a => a.Comment)
                .HasMaxLength(1000)
                .IsRequired(false);

            // Связи
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Tariffs)
                .WithOne(t => t.AppUser)
                .HasForeignKey(t => t.AppUserId)
                .OnDelete(DeleteBehavior.SetNull);

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
                .OnDelete(DeleteBehavior.SetNull);

            // Уникальные индексы
            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Начальные данные - Тарифы
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
                },
                new Tariff
                {
                    Id = 4,
                    Name = "Эконом",
                    Description = "Минимальный пакет",
                    MonthlyFee = 200.00m,
                    InternetTrafficGB = 2,
                    MinutesCount = 50,
                    SmsCount = 20
                }
            );

            // Начальные данные - Услуги
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
                },
                new Service
                {
                    Id = 4,
                    Name = "Международные звонки",
                    Description = "Звонки за границу",
                    Type = ServiceType.Calls,
                    Cost = 80.00m,
                    BillingPeriod = BillingPeriod.Monthly
                }
            );

            // Начальные данные - Пользователи
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    FullName = "Администратор Системы",
                    PhoneNumber = "9998887766",
                    Email = "admin@mobileoperator.ru",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Address = "г. Москва, ул. Административная, д. 1",
                    PassportData = "0000 000000",
                    Balance = 10000.00m,
                    Role = "Admin",
                    IsActive = true,
                    RegistrationDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    RefreshToken = null, // Явно указываем null
                    RefreshTokenExpiryTime = null,
                    LastLoginDate = null
                },
                new AppUser
                {
                    Id = 2,
                    FullName = "Иванов Иван Иванович",
                    PhoneNumber = "9991112233",
                    Email = "ivanov@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                    Address = "г. Москва, ул. Примерная, д. 10",
                    PassportData = "1234 567890",
                    Balance = 1500.50m,
                    Role = "User",
                    IsActive = true,
                    RegistrationDate = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    RefreshToken = null,
                    RefreshTokenExpiryTime = null,
                    LastLoginDate = null
                },
                new AppUser
                {
                    Id = 3,
                    FullName = "Петрова Анна Сергеевна",
                    PhoneNumber = "9992223344",
                    Email = "petrova@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User456!"),
                    Address = "г. Санкт-Петербург, ул. Тестовая, д. 5",
                    PassportData = "5678 901234",
                    Balance = 750.25m,
                    Role = "User",
                    IsActive = true,
                    RegistrationDate = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    RefreshToken = null,
                    RefreshTokenExpiryTime = null,
                    LastLoginDate = null
                }
            );

            // Начальные данные - Платежи
            modelBuilder.Entity<Payment>().HasData(
                new Payment
                {
                    Id = 1,
                    AppUserId = 2,
                    Amount = 1000.00m,
                    PaymentDateTime = new DateTime(2024, 2, 5, 10, 30, 0, DateTimeKind.Utc),
                    PaymentMethod = PaymentMethod.CreditCard,
                    Status = PaymentStatus.Completed
                },
                new Payment
                {
                    Id = 2,
                    AppUserId = 2,
                    Amount = 500.50m,
                    PaymentDateTime = new DateTime(2024, 3, 10, 14, 15, 0, DateTimeKind.Utc),
                    PaymentMethod = PaymentMethod.Online,
                    Status = PaymentStatus.Completed
                },
                new Payment
                {
                    Id = 3,
                    AppUserId = 3,
                    Amount = 1000.00m,
                    PaymentDateTime = new DateTime(2024, 3, 15, 9, 45, 0, DateTimeKind.Utc),
                    PaymentMethod = PaymentMethod.BankTransfer,
                    Status = PaymentStatus.Completed
                }
            );

            // Начальные данные - Заявки
            modelBuilder.Entity<Application>().HasData(
                new Application
                {
                    Id = 1,
                    AppUserId = 2,
                    Type = ApplicationType.NewConnection,
                    CreationDate = new DateTime(2024, 2, 1, 9, 0, 0, DateTimeKind.Utc),
                    ProcessingDate = new DateTime(2024, 2, 2, 14, 30, 0, DateTimeKind.Utc),
                    Status = ApplicationStatus.Completed,
                    Comment = "Новое подключение обработано"
                },
                new Application
                {
                    Id = 2,
                    AppUserId = 3,
                    Type = ApplicationType.TariffChange,
                    CreationDate = new DateTime(2024, 3, 5, 11, 20, 0, DateTimeKind.Utc),
                    Status = ApplicationStatus.InProgress,
                    Comment = "Запрос на смену тарифа"
                }
            );
        }
    }
}