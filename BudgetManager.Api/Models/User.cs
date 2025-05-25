namespace BudgetManager.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<Category> Categories { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<SavingGoal> SavingGoals { get; set; }
    }
} 