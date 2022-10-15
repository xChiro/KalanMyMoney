using System.Collections.ObjectModel;
using KalanMoney.Domain.Entities;

namespace KalanMoney.Domain.UseCases.GetAccountDashboard;

public record CategoriesBalances
{
    public IReadOnlyDictionary<string, decimal> Values { get; }
    
    private CategoriesBalances(IDictionary<string, decimal> values)
    {
        Values = new ReadOnlyDictionary<string, decimal>(values);
    }

    public static CategoriesBalances CreateFromTransactions(TransactionCollection transactionCollection)
    {
        var categories = new Dictionary<string, decimal>();

        foreach (var transaction in transactionCollection.Items)
        {
            if (categories.ContainsKey(transaction.Category.Value.ToLower()))
            {
                categories[transaction.Category.Value] += transaction.Amount;
            }
            else
            {
                categories.Add(transaction.Category.Value.ToLower(), transaction.Amount);
            }
        }

        return new CategoriesBalances(categories);
    }
}