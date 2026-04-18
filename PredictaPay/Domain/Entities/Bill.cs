using System.ComponentModel.DataAnnotations;
using PredictaPay.Domain.Enums;

namespace PredictaPay.Domain.Entities;

public class Bill
{
    public int BillId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(150)]
    public string BillName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }

    public DateOnly DueDate { get; set; }

    public RecurrenceType RecurrenceType { get; set; }

    public BillStatus Status { get; set; } = BillStatus.Pending;

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public AppUser? User { get; set; }

    public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}