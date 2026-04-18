using System.ComponentModel.DataAnnotations;
using PredictaPay.Domain.Enums;

namespace PredictaPay.Domain.Entities;

public class Reminder
{
    public int ReminderId { get; set; }

    [Required]
    public int BillId { get; set; }

    public DateTime ReminderDate { get; set; }

    public ReminderType ReminderType { get; set; }

    public bool IsSent { get; set; }

    public Bill? Bill { get; set; }
}