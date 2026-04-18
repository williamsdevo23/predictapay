using Microsoft.EntityFrameworkCore;
using PredictaPay.Domain.Entities;
using PredictaPay.Domain.Enums;

namespace PredictaPay.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();

    public DbSet<Bill> Bills => Set<Bill>();

    public DbSet<Reminder> Reminders => Set<Reminder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed sample data
        SeedData(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(user => user.UserId);
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(bill => bill.BillId);
            entity.Property(bill => bill.Amount).HasPrecision(18, 2);
            entity.Property(bill => bill.DueDate).HasColumnType("TEXT");
            entity.Property(bill => bill.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(bill => bill.User)
                .WithMany(user => user.Bills)
                .HasForeignKey(bill => bill.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(reminder => reminder.ReminderId);
            entity.Property(reminder => reminder.ReminderDate).HasColumnType("TEXT");

            entity.HasOne(reminder => reminder.Bill)
                .WithMany(bill => bill.Reminders)
                .HasForeignKey(reminder => reminder.BillId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Create sample users
        var u1 = new AppUser
        {
            UserId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PasswordHash = "AQAAAAIAAYagAAAAENptXfHm/1aTmuwLuGUoG78eSrRuQLrOfsXKEk1iMO8UXwG0mBLgtUEpid9cz5O5Qw==",
            Role = "User",
            CreatedAt = new DateTime(2026, 3, 19, 3, 47, 42, 860, DateTimeKind.Utc).AddTicks(6430)
        };

        var u2 = new AppUser
        {
            UserId = 2,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@example.com",
            PasswordHash = "AQAAAAIAAYagAAAAEKHbVV6MCS3u4bp103KbyBB7uYL0RNkbUY8RA1x/StTJQtXuzNT2oZ2fQZ369trCNA==",
            Role = "User",
            CreatedAt = new DateTime(2026, 3, 29, 3, 47, 42, 966, DateTimeKind.Utc).AddTicks(470)
        };

        modelBuilder.Entity<AppUser>().HasData(u1, u2);

        // Create sample bills
        var bill1 = new Bill
        {
            BillId = 1,
            UserId = u1.UserId,
            BillName = "Electricity Bill",
            Category = "Utilities",
            Amount = 125.50m,
            DueDate = new DateOnly(2026, 4, 22),
            RecurrenceType = RecurrenceType.Monthly,
            Status = BillStatus.Pending,
            Notes = "Monthly electric bill",
            CreatedAt = new DateTime(2026, 4, 8, 3, 47, 42, 983, DateTimeKind.Utc).AddTicks(330)
        };

        var bill2 = new Bill
        {
            BillId = 2,
            UserId = u1.UserId,
            BillName = "Internet Bill",
            Category = "Utilities",
            Amount = 79.99m,
            DueDate = new DateOnly(2026, 4, 20),
            RecurrenceType = RecurrenceType.Monthly,
            Status = BillStatus.Pending,
            Notes = "Monthly internet service",
            CreatedAt = new DateTime(2026, 4, 3, 3, 47, 42, 983, DateTimeKind.Utc).AddTicks(1010)
        };

        var bill3 = new Bill
        {
            BillId = 3,
            UserId = u2.UserId,
            BillName = "Car Insurance",
            Category = "Insurance",
            Amount = 150.00m,
            DueDate = new DateOnly(2026, 4, 27),
            RecurrenceType = RecurrenceType.Monthly,
            Status = BillStatus.Paid,
            Notes = "Auto insurance premium",
            CreatedAt = new DateTime(2026, 4, 13, 3, 47, 42, 983, DateTimeKind.Utc).AddTicks(1030)
        };

        modelBuilder.Entity<Bill>().HasData(bill1, bill2, bill3);

        // Create sample reminders
        var reminder1 = new Reminder
        {
            ReminderId = 1,
            BillId = bill1.BillId,
            ReminderDate = new DateTime(2026, 4, 20, 3, 47, 42, 983, DateTimeKind.Utc).AddTicks(5470),
            ReminderType = ReminderType.Email,
            IsSent = false
        };

        var reminder2 = new Reminder
        {
            ReminderId = 2,
            BillId = bill2.BillId,
            ReminderDate = new DateTime(2026, 4, 19, 3, 47, 42, 983, DateTimeKind.Utc).AddTicks(7600),
            ReminderType = ReminderType.Push,
            IsSent = false
        };

        var reminder3 = new Reminder
        {
            ReminderId = 3,
            BillId = bill3.BillId,
            ReminderDate = new DateTime(2026, 4, 25, 3, 47, 42, 983, DateTimeKind.Utc).AddTicks(7610),
            ReminderType = ReminderType.Email,
            IsSent = true
        };

        modelBuilder.Entity<Reminder>().HasData(reminder1, reminder2, reminder3);
    }
}