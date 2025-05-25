using Microsoft.AspNetCore.Mvc;
using BudgetManager.Api.Data;
using BudgetManager.Api.DTOs;
using BudgetManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly BudgetDbContext _context;

        public ExpenseController(BudgetDbContext context)
        {
            _context = context;
        }

        // GET: api/expense
        [HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            var expenses = await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Category.Type == "Expense")
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Date = t.Date,
                    Description = t.Description,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name
                })
                .ToListAsync();
            return Ok(expenses);
        }

        // GET: api/expense/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpense(int id)
        {
            var expense = await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Category.Type == "Expense" && t.Id == id)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Date = t.Date,
                    Description = t.Description,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name
                })
                .FirstOrDefaultAsync();
            if (expense == null) return NotFound();
            return Ok(expense);
        }

        // POST: api/expense
        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] TransactionDto dto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.Type == "Expense");
            if (category == null)
                return BadRequest("Invalid category for expense.");
            var expense = new Transaction
            {
                Amount = dto.Amount,
                Date = dto.Date,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                UserId = 1 // TODO: Zmień na aktualnego użytkownika po wdrożeniu autoryzacji
            };
            _context.Transactions.Add(expense);
            await _context.SaveChangesAsync();
            dto.Id = expense.Id;
            dto.CategoryName = category.Name;
            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, dto);
        }

        // PUT: api/expense/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] TransactionDto dto)
        {
            var expense = await _context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id && t.Category.Type == "Expense");
            if (expense == null) return NotFound();
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.Type == "Expense");
            if (category == null) return BadRequest("Invalid category for expense.");
            expense.Amount = dto.Amount;
            expense.Date = dto.Date;
            expense.Description = dto.Description;
            expense.CategoryId = dto.CategoryId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/expense/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id && t.Category.Type == "Expense");
            if (expense == null) return NotFound();
            _context.Transactions.Remove(expense);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 