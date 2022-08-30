using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Persistence.CosmosDB.DTOs;

public class FinancialAccountDto
{
    public FinancialAccountDto(string id, string name, OwnerDto owner, long creationTimeStamp,
        List<TransactionDto> transactions, decimal balance)
    {
        Id = id;
        Name = name;
        Owner = owner;
        CreationTimeStamp = creationTimeStamp;
        Transactions = transactions;
        Balance = balance;
    }

    public string Id { get; }

    public string Name { get; }

    public OwnerDto Owner { get; }

    public decimal Balance { get; }

    public long CreationTimeStamp { get; }

    public List<TransactionDto> Transactions { get; }

    public static FinancialAccountDto FromFinancialAccount(FinancialAccount financialAccount)
    {
        var ownerDto = new OwnerDto(financialAccount.Owner.SubId, financialAccount.Owner.Name);
        var transactions = financialAccount.Transactions.Items.Select(currentTransaction =>
            new TransactionDto(currentTransaction.Id, currentTransaction.Amount, currentTransaction.Description.Value,
                currentTransaction.Category.Value, currentTransaction.TimeStamp.Value)).ToList();

        return new FinancialAccountDto(financialAccount.Id, financialAccount.Name.Value, ownerDto,
            financialAccount.CreationDate.Value, transactions, financialAccount.Balance.Amount);
    }

    public FinancialAccount ToFinancialAccount()
    {
        var owner = new Owner(Owner.SubId, Owner.Name);

        var transactions = Transactions.Select(currentTransaction =>
                new Transaction(currentTransaction.Id, currentTransaction.Amount,
                    Description.Create(currentTransaction.Description),
                    Category.Create(currentTransaction.Category),
                    new TimeStamp(currentTransaction.TimeStamp)))
            .ToList();


        return new FinancialAccount(Id, AccountName.Create(Name), owner, Balance, new TimeStamp(CreationTimeStamp),
            transactions);
    }
}