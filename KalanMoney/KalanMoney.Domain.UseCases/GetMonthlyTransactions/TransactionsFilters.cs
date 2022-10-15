namespace KalanMoney.Domain.UseCases.GetMonthlyTransactions;

public record TransactionsFilters
{
    public TransactionsFilters(int year, int month, string? category = null)
    {
        if (month is < 1 or > 12) throw new IndexOutOfRangeException("Invalid month number");
        if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
            throw new IndexOutOfRangeException("Invalid year number");
        
        Year = year;
        Month = month;
        Category = category;
    }

    public int Year { get; init; }
    public int Month { get; init; }
    public string? Category { get; init; }
}