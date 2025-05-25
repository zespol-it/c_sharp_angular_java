using System;

namespace BudgetManager.Api.DTOs
{
    public class SavingGoalDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime EndDate { get; set; }
    }
} 