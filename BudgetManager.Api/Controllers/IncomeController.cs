using Microsoft.AspNetCore.Mvc;
using BudgetManager.Api.Data;
using BudgetManager.Api.DTOs;
using BudgetManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly BudgetDbContext _context;

        public IncomeController(BudgetDbContext context)
        {
            _context = context;
        }

        // GET: api/income
        [HttpGet]
        public async Task<IActionResult> GetIncomes()
        {
            var incomes = await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Category.Type == "Income")
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
            return Ok(incomes);
        }

        // GET: api/income/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncome(int id)
        {
            var income = await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Category.Type == "Income" && t.Id == id)
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
            if (income == null) return NotFound();
            return Ok(income);
        }

        // POST: api/income
        [HttpPost]
        public async Task<IActionResult> CreateIncome([FromBody] TransactionDto dto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.Type == "Income");
            if (category == null)
                return BadRequest("Invalid category for income.");
            var income = new Transaction
            {
                Amount = dto.Amount,
                Date = dto.Date,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                UserId = 1 // TODO: Zmień na aktualnego użytkownika po wdrożeniu autoryzacji
            };
            _context.Transactions.Add(income);
            await _context.SaveChangesAsync();
            dto.Id = income.Id;
            dto.CategoryName = category.Name;
            return CreatedAtAction(nameof(GetIncome), new { id = income.Id }, dto);
        }

        // PUT: api/income/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncome(int id, [FromBody] TransactionDto dto)
        {
            var income = await _context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id && t.Category.Type == "Income");
            if (income == null) return NotFound();
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.Type == "Income");
            if (category == null) return BadRequest("Invalid category for income.");
            income.Amount = dto.Amount;
            income.Date = dto.Date;
            income.Description = dto.Description;
            income.CategoryId = dto.CategoryId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/income/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            var income = await _context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id && t.Category.Type == "Income");
            if (income == null) return NotFound();
            _context.Transactions.Remove(income);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 