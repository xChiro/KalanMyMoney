using System;

namespace KalanMoney.API.Functions.Commons;

public record TransactionResponse(decimal Amount, string Description, string Category, DateTime Time);