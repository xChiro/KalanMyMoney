using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.UseCases.Repositories.Models;

public record AddTransactionAccountModel(string AccountId, Balance Balance);