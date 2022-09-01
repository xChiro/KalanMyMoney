namespace KalanMoney.Domain.UseCases.Repositories.Models;

public record TransactionFilter(DateOnly From, DateOnly To)
{
    public static TransactionFilter CreateMonthRangeFromUtcNow()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        return new TransactionFilter(today.AddDays(-30), today);
    }

    public static TransactionFilter CreateMonthRange(int year, int month)
    {
        var to = new DateOnly(year, month, DateTime.DaysInMonth(year, month));
        var from = new DateOnly(year, month, DateOnly.MinValue.Day);
        
        return new TransactionFilter(from, to);
    }
}