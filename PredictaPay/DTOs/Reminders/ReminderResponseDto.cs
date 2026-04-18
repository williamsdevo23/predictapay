using PredictaPay.Domain.Enums;

namespace PredictaPay.DTOs.Reminders;

public class ReminderResponseDto
{
    public int ReminderId { get; set; }

    public int BillId { get; set; }

    public DateTime ReminderDate { get; set; }

    public ReminderType ReminderType { get; set; }

    public bool IsSent { get; set; }
}