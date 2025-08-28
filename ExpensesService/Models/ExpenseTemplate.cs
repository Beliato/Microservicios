namespace ExpensesService.Models;

public class ExpenseTemplate
{
    public int Id { get; set; }
    public int AccountOriginId { get; set; }
    public int AccountDestinationId { get; set; }
    public int PaymentDay { get; set; }  // día fijo del mes (1-31)
    public string Category { get; set; } = string.Empty;
    public decimal BudgetedAmount { get; set; }
}
