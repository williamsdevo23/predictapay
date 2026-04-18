using PredictaPay.Domain.Enums;

namespace PredictaPay.DTOs.Reminders;

public class CreateReminderRequestDto
{
    public DateTime ReminderDate { get; set; }

    public ReminderType ReminderType { get; set; }
}