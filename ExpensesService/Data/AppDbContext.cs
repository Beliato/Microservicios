using Microsoft.EntityFrameworkCore;
using ExpensesService.Models;

namespace ExpensesService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<ExpenseTemplate> ExpenseTemplates { get; set; }
    public DbSet<Expense> Expenses { get; set; }
}
