namespace ExpensesService.Models;

public class Account
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Ahorro, Corriente, Tarjeta
}
