using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PredictaPay.Domain.Entities;
using PredictaPay.DTOs.Reminders;
using PredictaPay.Infrastructure.Data;

namespace PredictaPay.Controllers;

[ApiController]
[Route("api/bills/{billId:int}/reminders")]
public class RemindersController : ControllerBase
{
    private readonly AppDbContext _context;

    public RemindersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("/api/reminders")]
    public async Task<ActionResult<IEnumerable<ReminderResponseDto>>> GetAllReminders(CancellationToken cancellationToken)
    {
        var reminders = await _context.Reminders
            .AsNoTracking()
            .OrderBy(reminder => reminder.ReminderDate)
            .Select(reminder => new ReminderResponseDto
            {
                ReminderId = reminder.ReminderId,
                BillId = reminder.BillId,
                ReminderDate = reminder.ReminderDate,
                ReminderType = reminder.ReminderType,
                IsSent = reminder.IsSent
            })
            .ToListAsync(cancellationToken);

        return Ok(reminders);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReminderResponseDto>>> GetReminders(int billId, CancellationToken cancellationToken)
    {
        var reminders = await _context.Reminders
            .AsNoTracking()
            .Where(reminder => reminder.BillId == billId)
            .OrderBy(reminder => reminder.ReminderDate)
            .Select(reminder => new ReminderResponseDto
            {
                ReminderId = reminder.ReminderId,
                BillId = reminder.BillId,
                ReminderDate = reminder.ReminderDate,
                ReminderType = reminder.ReminderType,
                IsSent = reminder.IsSent
            })
            .ToListAsync(cancellationToken);

        return Ok(reminders);
    }

    [HttpPost]
    public async Task<ActionResult<ReminderResponseDto>> CreateReminder(int billId, [FromBody] CreateReminderRequestDto request, CancellationToken cancellationToken)
    {
        var billExists = await _context.Bills.AnyAsync(bill => bill.BillId == billId, cancellationToken);
        if (!billExists)
        {
            return NotFound($"Bill '{billId}' was not found.");
        }

        var reminder = new Reminder
        {
            BillId = billId,
            ReminderDate = request.ReminderDate,
            ReminderType = request.ReminderType,
            IsSent = false
        };

        _context.Reminders.Add(reminder);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetReminders), new { billId }, new ReminderResponseDto
        {
            ReminderId = reminder.ReminderId,
            BillId = reminder.BillId,
            ReminderDate = reminder.ReminderDate,
            ReminderType = reminder.ReminderType,
            IsSent = reminder.IsSent
        });
    }

    [HttpPatch("{reminderId:int}/sent")]
    public async Task<IActionResult> MarkReminderAsSent(int billId, int reminderId, CancellationToken cancellationToken)
    {
        var reminder = await _context.Reminders
            .FirstOrDefaultAsync(item => item.ReminderId == reminderId && item.BillId == billId, cancellationToken);

        if (reminder is null)
        {
            return NotFound();
        }

        reminder.IsSent = true;
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}