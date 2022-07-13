using KalanMoney.Persistence.MemoryDatabase.DTOs;

namespace KalanMoney.Persistence.MemoryDatabase;

public class MemoryDb
{
    public Dictionary<string, FinancialAccountModel> FinancialAccounts { get; }
    
    public MemoryDb()
    {
        FinancialAccounts = new Dictionary<string, FinancialAccountModel>();
    }

    public MemoryDb(FinancialAccountModel financialAccountModel)
    {
        FinancialAccounts = new Dictionary<string, FinancialAccountModel>()
            {{financialAccountModel.Id, financialAccountModel}};
    }
}