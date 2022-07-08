using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.UseCases.Repositories.Models;

public record AddTransactionModel(string AccountId, Balance AccountBalance, string CategoryId, Balance CategoryBalance);