using ExpensesService.Data;
using ExpensesService.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Conexión SQL Server (ajustaremos con Docker después)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ExpensesDb")); // de momento en memoria

var app = builder.Build();

// Endpoints Accounts
app.MapGet("/accounts", async (AppDbContext db) => await db.Accounts.ToListAsync());

app.MapPost("/accounts", async (AppDbContext db, Account account) =>
{
    db.Accounts.Add(account);
    await db.SaveChangesAsync();
    return Results.Created($"/accounts/{account.Id}", account);
});

// Endpoints Expense Templates
app.MapGet("/templates", async (AppDbContext db) => await db.ExpenseTemplates.ToListAsync());

app.MapPost("/templates", async (AppDbContext db, ExpenseTemplate template) =>
{
    db.ExpenseTemplates.Add(template);
    await db.SaveChangesAsync();
    return Results.Created($"/templates/{template.Id}", template);
});

app.Run();
