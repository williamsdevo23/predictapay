using System.ComponentModel.DataAnnotations;
using PredictaPay.Domain.Enums;

namespace PredictaPay.DTOs.Bills;

public class UpdateBillRequestDto
{
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

    public BillStatus Status { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}