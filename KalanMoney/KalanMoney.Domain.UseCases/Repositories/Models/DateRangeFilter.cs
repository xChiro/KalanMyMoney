namespace KalanMoney.Domain.UseCases.Repositories.Models;

public record DateRangeFilter(DateOnly From, DateOnly To)
{
    public static DateRangeFilter CreateMonthRangeFromUtcNow()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        return new DateRangeFilter(today.AddDays(-30), today);
    }

    public static DateRangeFilter CreateMonthRange(int year, int month)
    {
        var to = new DateOnly(year, month, DateTime.DaysInMonth(year, month));
        var from = new DateOnly(year, month, DateOnly.MinValue.Day);
        
        return new DateRangeFilter(from, to);
    }
}