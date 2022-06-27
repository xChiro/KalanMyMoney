using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.UseCases.Repositories.Models;

public record AddTransactionCategoryModel(string CategoryId, Balance Balance);