namespace KalanMoney.Domain.UseCases.Repositories.Models;

public record TransactionFilters(DateOnly From, DateOnly To)
{
    public static TransactionFilters CreateLastMonthRange()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        return new TransactionFilters(today, today.AddDays(-30));
    }
}