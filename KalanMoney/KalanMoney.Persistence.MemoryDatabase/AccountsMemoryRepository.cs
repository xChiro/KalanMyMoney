using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Persistence.MemoryDatabase.DTOs;

namespace KalanMoney.Persistence.MemoryDatabase;

public class AccountsMemoryRepository : IAccountCommandsRepository, IAccountQueriesRepository
{
    public Dictionary<string, FinancialAccountModel> FinancialAccounts { get; }

    public AccountsMemoryRepository()
    {
        FinancialAccounts = new Dictionary<string, FinancialAccountModel>();
    }

    public AccountsMemoryRepository(FinancialAccountModel financialAccountModel)
    {
        FinancialAccounts = new Dictionary<string, FinancialAccountModel>()
            {{financialAccountModel.Id, financialAccountModel}};
    }

    public void OpenAccount(FinancialAccount account)
    {
        FinancialAccounts.Add(account.Id, FinancialAccountModel.CreateFromFinancialAccount(account));
    }

    public void AddTransaction(AddTransactionModel addTransactionModel, Transaction transaction)
    {
        FinancialAccounts[addTransactionModel.AccountId].Balance = addTransactionModel.AccountBalance.Amount;
        FinancialAccounts[addTransactionModel.AccountId].Transactions.Add(transaction);
        FinancialAccounts[addTransactionModel.AccountId].CategoryModels[addTransactionModel.CategoryId].Balance =
            addTransactionModel.CategoryBalance.Amount;
        FinancialAccounts[addTransactionModel.AccountId].CategoryModels[addTransactionModel.CategoryId].Transactions
            .Add(transaction);
    }

    public FinancialAccount? GetAccount(string id, TransactionFilter transactionFilter)
    {
        if (!FinancialAccounts.TryGetValue(id, out var financialAccountModel)) return null;

        var transactions = ApplyFilters(transactionFilter, financialAccountModel);
        return  FinancialAccountModel.ToFinancialAccount(financialAccountModel, transactions);
    }

    public FinancialAccount? GetAccountByOwner(string ownerId, TransactionFilter transactionFilter)
    {
        var accountModel = FinancialAccounts.Where(x => x.Value.OwnerId == ownerId)
            .Select(x => x.Value).FirstOrDefault();

        if (accountModel == null) return null;
        
        var transactions = ApplyFilters(transactionFilter, accountModel);
        return FinancialAccountModel.ToFinancialAccount(accountModel, transactions);

    }

    public FinancialAccount? GetAccountOnly(string id)
    {
        if(!FinancialAccounts.TryGetValue(id, out var financialAccountModel)) return null;
        financialAccountModel.Transactions = new List<Transaction>();
        
        return financialAccountModel.ToFinancialAccount();
    }

    private static IEnumerable<Transaction> ApplyFilters(TransactionFilter transactionFilter, FinancialAccountModel financialAccountModel)
    {
        return financialAccountModel.Transactions.Where(x =>
            x.TimeStamp.ToDateTime() >= transactionFilter.From.ToDateTime(TimeOnly.MinValue) &&  
            x.TimeStamp.ToDateTime() <= transactionFilter.To.ToDateTime(TimeOnly.MaxValue));
    }

    public int ItemsCount()
    {
        return FinancialAccounts.Count;
    }
}