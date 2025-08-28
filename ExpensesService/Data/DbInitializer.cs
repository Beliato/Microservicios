using Microsoft.EntityFrameworkCore;

namespace ExpensesService.Data;

public static class DbInitializer
{
    public static void MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
}
