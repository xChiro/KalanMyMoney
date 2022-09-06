using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.UseCases.Repositories.Models;

public record GetTransactionsFilters(DateRangeFilter RangeFilter, Category[]? Categories);