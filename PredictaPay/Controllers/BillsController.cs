using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PredictaPay.Domain.Entities;
using PredictaPay.DTOs.Bills;
using PredictaPay.Infrastructure.Data;

namespace PredictaPay.Controllers;

[ApiController]
[Route("api/users/{userId:int}/bills")]
public class BillsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BillsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("/api/bills")]
    public async Task<ActionResult<IEnumerable<BillResponseDto>>> GetAllBills(CancellationToken cancellationToken)
    {
        var bills = await _context.Bills
            .AsNoTracking()
            .OrderByDescending(bill => bill.CreatedAt)
            .Select(ToResponseProjection())
            .ToListAsync(cancellationToken);

        return Ok(bills);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BillResponseDto>>> GetBillsForUser(int userId, CancellationToken cancellationToken)
    {
        var bills = await _context.Bills
            .AsNoTracking()
            .Where(bill => bill.UserId == userId)
            .OrderByDescending(bill => bill.CreatedAt)
            .Select(ToResponseProjection())
            .ToListAsync(cancellationToken);

        return Ok(bills);
    }

    [HttpGet("{billId:int}")]
    public async Task<ActionResult<BillResponseDto>> GetBill(int userId, int billId, CancellationToken cancellationToken)
    {
        var bill = await _context.Bills
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.BillId == billId && item.UserId == userId, cancellationToken);

        if (bill is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(bill));
    }

    [HttpPost]
    public async Task<ActionResult<BillResponseDto>> CreateBill(int userId, [FromBody] CreateBillRequestDto request, CancellationToken cancellationToken)
    {
        var userExists = await _context.Users.AnyAsync(user => user.UserId == userId, cancellationToken);
        if (!userExists)
        {
            return NotFound($"User '{userId}' was not found.");
        }

        var bill = new Bill
        {
            UserId = userId,
            BillName = request.BillName,
            Category = request.Category,
            Amount = request.Amount,
            DueDate = request.DueDate,
            RecurrenceType = request.RecurrenceType,
            Notes = request.Notes,
            Status = Domain.Enums.BillStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Bills.Add(bill);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetBill), new { userId, billId = bill.BillId }, ToResponse(bill));
    }

    [HttpPut("{billId:int}")]
    public async Task<IActionResult> UpdateBill(int userId, int billId, [FromBody] UpdateBillRequestDto request, CancellationToken cancellationToken)
    {
        var bill = await _context.Bills
            .FirstOrDefaultAsync(item => item.BillId == billId && item.UserId == userId, cancellationToken);

        if (bill is null)
        {
            return NotFound();
        }

        bill.BillName = request.BillName;
        bill.Category = request.Category;
        bill.Amount = request.Amount;
        bill.DueDate = request.DueDate;
        bill.RecurrenceType = request.RecurrenceType;
        bill.Status = request.Status;
        bill.Notes = request.Notes;

        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{billId:int}")]
    public async Task<IActionResult> DeleteBill(int userId, int billId, CancellationToken cancellationToken)
    {
        var bill = await _context.Bills
            .FirstOrDefaultAsync(item => item.BillId == billId && item.UserId == userId, cancellationToken);

        if (bill is null)
        {
            return NotFound();
        }

        _context.Bills.Remove(bill);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private static BillResponseDto ToResponse(Bill bill)
    {
        return new BillResponseDto
        {
            BillId = bill.BillId,
            UserId = bill.UserId,
            BillName = bill.BillName,
            Category = bill.Category,
            Amount = bill.Amount,
            DueDate = bill.DueDate,
            RecurrenceType = bill.RecurrenceType,
            Status = bill.Status,
            Notes = bill.Notes,
            CreatedAt = bill.CreatedAt
        };
    }

    private static System.Linq.Expressions.Expression<Func<Bill, BillResponseDto>> ToResponseProjection()
    {
        return bill => new BillResponseDto
        {
            BillId = bill.BillId,
            UserId = bill.UserId,
            BillName = bill.BillName,
            Category = bill.Category,
            Amount = bill.Amount,
            DueDate = bill.DueDate,
            RecurrenceType = bill.RecurrenceType,
            Status = bill.Status,
            Notes = bill.Notes,
            CreatedAt = bill.CreatedAt
        };
    }
}