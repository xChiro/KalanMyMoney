namespace KalanMoney.Domain.UseCases.AccountDashboard;

public interface IAccountDashboardInput
{
    public void Execute(string ownerId, IAccountDashboardOutput output);
}