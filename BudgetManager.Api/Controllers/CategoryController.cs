using Microsoft.AspNetCore.Mvc;
using BudgetManager.Api.Data;
using BudgetManager.Api.DTOs;
using BudgetManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly BudgetDbContext _context;

        public CategoryController(BudgetDbContext context)
        {
            _context = context;
        }

        // GET: api/category
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type
                })
                .ToListAsync();
            return Ok(categories);
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type
                })
                .FirstOrDefaultAsync();
            if (category == null) return NotFound();
            return Ok(category);
        }

        // POST: api/category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)
        {
            if (dto.Type != "Income" && dto.Type != "Expense")
                return BadRequest("Type must be 'Income' or 'Expense'.");
            var category = new Category
            {
                Name = dto.Name,
                Type = dto.Type,
                UserId = 1 // TODO: Zmień na aktualnego użytkownika po wdrożeniu autoryzacji
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            dto.Id = category.Id;
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, dto);
        }

        // PUT: api/category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            if (dto.Type != "Income" && dto.Type != "Expense")
                return BadRequest("Type must be 'Income' or 'Expense'.");
            category.Name = dto.Name;
            category.Type = dto.Type;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 