using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Dodaję politykę CORS dla frontendu Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core + SQLite
builder.Services.AddDbContext<BudgetManager.Api.Data.BudgetDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Auth
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddScoped<BudgetManager.Api.Services.IUserService, BudgetManager.Api.Services.UserService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Włączam CORS dla frontendu Angular
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Dodaję prosty endpoint na /, aby nie było 404 na stronie głównej
app.MapGet("/", () => Results.Ok("BudgetManager API działa!"));

// SEEDING SAMPLE DATA
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BudgetManager.Api.Data.BudgetDbContext>();
    db.Database.EnsureCreated();

    if (!db.Users.Any())
    {
        var user = new BudgetManager.Api.Models.User
        {
            Email = "test@example.com",
            PasswordHash = "testhash",
            FirstName = "Jan",
            LastName = "Kowalski"
        };
        db.Users.Add(user);
        db.SaveChanges();

        var cat1 = new BudgetManager.Api.Models.Category { Name = "Zakupy", Type = "Expense", UserId = user.Id };
        var cat2 = new BudgetManager.Api.Models.Category { Name = "Wynagrodzenie", Type = "Income", UserId = user.Id };
        db.Categories.AddRange(cat1, cat2);
        db.SaveChanges();

        var t1 = new BudgetManager.Api.Models.Transaction
        {
            Amount = 150.00m,
            Date = DateTime.Now.AddDays(-2),
            Description = "Zakupy spożywcze",
            CategoryId = cat1.Id,
            UserId = user.Id
        };
        var t2 = new BudgetManager.Api.Models.Transaction
        {
            Amount = 4000.00m,
            Date = DateTime.Now.AddDays(-10),
            Description = "Wypłata",
            CategoryId = cat2.Id,
            UserId = user.Id
        };
        db.Transactions.AddRange(t1, t2);
        db.SaveChanges();

        var goal = new BudgetManager.Api.Models.SavingGoal
        {
            Name = "Wakacje",
            TargetAmount = 5000,
            CurrentAmount = 1200,
            EndDate = DateTime.Now.AddMonths(6),
            UserId = user.Id
        };
        db.SavingGoals.Add(goal);
        db.SaveChanges();

        var cat3 = new BudgetManager.Api.Models.Category { Name = "Restauracja", Type = "Expense", UserId = user.Id };
        var cat4 = new BudgetManager.Api.Models.Category { Name = "Premia", Type = "Income", UserId = user.Id };
        db.Categories.AddRange(cat3, cat4);
        db.SaveChanges();

        var t3 = new BudgetManager.Api.Models.Transaction
        {
            Amount = 80.00m,
            Date = DateTime.Now.AddDays(-1),
            Description = "Obiad w restauracji",
            CategoryId = cat3.Id,
            UserId = user.Id
        };
        var t4 = new BudgetManager.Api.Models.Transaction
        {
            Amount = 500.00m,
            Date = DateTime.Now.AddDays(-5),
            Description = "Premia miesięczna",
            CategoryId = cat4.Id,
            UserId = user.Id
        };
        db.Transactions.AddRange(t3, t4);
        db.SaveChanges();

        var goal2 = new BudgetManager.Api.Models.SavingGoal
        {
            Name = "Nowy laptop",
            TargetAmount = 4000,
            CurrentAmount = 500,
            EndDate = DateTime.Now.AddMonths(12),
            UserId = user.Id
        };
        db.SavingGoals.Add(goal2);
        db.SaveChanges();
    }
}

app.Run();
