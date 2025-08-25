namespace ExpensesService.Models;

public class ExpenseTemplate
{
    public int Id { get; set; }
    public int AccountOriginId { get; set; }
    public int AccountDestinationId { get; set; }
    public string Rubro { get; set; } = string.Empty;
    public int DiaPago { get; set; }
    public decimal MontoPresupuestado { get; set; }

    public Account? AccountOrigin { get; set; }
    public Account? AccountDestination { get; set; }
}
