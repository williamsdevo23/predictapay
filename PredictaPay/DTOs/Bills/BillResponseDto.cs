using PredictaPay.Domain.Enums;

namespace PredictaPay.DTOs.Bills;

public class BillResponseDto
{
    public int BillId { get; set; }

    public int UserId { get; set; }

    public string BillName { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateOnly DueDate { get; set; }

    public RecurrenceType RecurrenceType { get; set; }

    public BillStatus Status { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
}