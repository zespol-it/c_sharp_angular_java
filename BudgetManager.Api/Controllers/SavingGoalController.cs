using Microsoft.AspNetCore.Mvc;
using BudgetManager.Api.Data;
using BudgetManager.Api.DTOs;
using BudgetManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavingGoalController : ControllerBase
    {
        private readonly BudgetDbContext _context;

        public SavingGoalController(BudgetDbContext context)
        {
            _context = context;
        }

        // GET: api/savinggoal
        [HttpGet]
        public async Task<IActionResult> GetSavingGoals()
        {
            var goals = await _context.SavingGoals
                .Select(g => new SavingGoalDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    TargetAmount = g.TargetAmount,
                    CurrentAmount = g.CurrentAmount,
                    EndDate = g.EndDate
                })
                .ToListAsync();
            return Ok(goals);
        }

        // GET: api/savinggoal/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSavingGoal(int id)
        {
            var goal = await _context.SavingGoals
                .Where(g => g.Id == id)
                .Select(g => new SavingGoalDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    TargetAmount = g.TargetAmount,
                    CurrentAmount = g.CurrentAmount,
                    EndDate = g.EndDate
                })
                .FirstOrDefaultAsync();
            if (goal == null) return NotFound();
            return Ok(goal);
        }

        // POST: api/savinggoal
        [HttpPost]
        public async Task<IActionResult> CreateSavingGoal([FromBody] SavingGoalDto dto)
        {
            var goal = new SavingGoal
            {
                Name = dto.Name,
                TargetAmount = dto.TargetAmount,
                CurrentAmount = dto.CurrentAmount,
                EndDate = dto.EndDate,
                UserId = 1 // TODO: Zmień na aktualnego użytkownika po wdrożeniu autoryzacji
            };
            _context.SavingGoals.Add(goal);
            await _context.SaveChangesAsync();
            dto.Id = goal.Id;
            return CreatedAtAction(nameof(GetSavingGoal), new { id = goal.Id }, dto);
        }

        // PUT: api/savinggoal/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSavingGoal(int id, [FromBody] SavingGoalDto dto)
        {
            var goal = await _context.SavingGoals.FirstOrDefaultAsync(g => g.Id == id);
            if (goal == null) return NotFound();
            goal.Name = dto.Name;
            goal.TargetAmount = dto.TargetAmount;
            goal.CurrentAmount = dto.CurrentAmount;
            goal.EndDate = dto.EndDate;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/savinggoal/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSavingGoal(int id)
        {
            var goal = await _context.SavingGoals.FirstOrDefaultAsync(g => g.Id == id);
            if (goal == null) return NotFound();
            _context.SavingGoals.Remove(goal);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 