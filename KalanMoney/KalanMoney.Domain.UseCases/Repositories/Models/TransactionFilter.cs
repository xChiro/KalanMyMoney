namespace KalanMoney.Domain.UseCases.Repositories.Models;

public record TransactionFilter(DateOnly From, DateOnly To)
{
    public static TransactionFilter CreateMonthRangeFromUtcNow()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        return new TransactionFilter(today, today.AddDays(-30));
    }
}