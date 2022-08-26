using System;

namespace KalanMoney.API.Functions.Commons;

public record TransactionResponse(string Id, decimal Amount, string Description, string Category, DateTime Time);