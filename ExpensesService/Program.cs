using ExpensesService.Data;
using ExpensesService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Conexión SQL Server (ajustaremos con Docker después)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? "Server=localhost,1433;Database=ExpensesDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Expenses API", Version = "v1" });
});

var app = builder.Build();

// Middleware de Swagger
if (app.Environment.IsDevelopment() || true) // forzamos siempre visible
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

// Endpoints Expenses

// Reinicia mes copiando templates
app.MapPost("/restart-month", async (AppDbContext db) =>
{
    var templates = await db.ExpenseTemplates.ToListAsync();

    foreach (var t in templates)
    {
        var expense = new Expense
        {
            AccountOriginId = t.AccountOriginId,
            AccountDestinationId = t.AccountDestinationId,
            PaymentDay = DateTime.UtcNow, // o lógica de mes actual
            Category = t.Category,
            BudgetedAmount = t.BudgetedAmount,
            TransferDone = false,
            TemplateId = t.Id
        };
        db.Expenses.Add(expense);
    }

    await db.SaveChangesAsync();
    return Results.Ok("Mes reiniciado");
});

// Listar gastos ordenados
app.MapGet("/expenses", async (AppDbContext db) =>
    await db.Expenses
        .OrderBy(e => e.TransferDone)
        .ThenBy(e => e.PaymentDay)
        .ToListAsync());

// Marcar gasto como efectuado
app.MapPut("/expenses/{id}/mark-done", async (int id, AppDbContext db, MarkDoneRequest request) =>
{
    var expense = await db.Expenses.FindAsync(id);
    if (expense == null) return Results.NotFound();

    expense.TransferDone = true;
    expense.RealAmount = request.RealAmount;
    await db.SaveChangesAsync();

    var ordered = await db.Expenses
        .OrderBy(e => e.TransferDone)
        .ThenBy(e => e.PaymentDay)
        .ToListAsync();

    return Results.Ok(ordered);
});

app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Services.GetRequiredService<IHost>().MigrateDatabase();
});

app.Run();
