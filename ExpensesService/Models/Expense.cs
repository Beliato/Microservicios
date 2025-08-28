namespace ExpensesService.Models;

public class Expense
{
    public int Id { get; set; }
    public int AccountOriginId { get; set; }
    public int AccountDestinationId { get; set; }
    public DateTime PaymentDay { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal BudgetedAmount { get; set; }
    public bool TransferDone { get; set; } = false;
    public decimal? RealAmount { get; set; }
    public decimal? Difference => RealAmount.HasValue ? BudgetedAmount - RealAmount.Value : null;
    public decimal? Savings => Difference > 0 ? Difference : 0;

    public int TemplateId { get; set; }
}
