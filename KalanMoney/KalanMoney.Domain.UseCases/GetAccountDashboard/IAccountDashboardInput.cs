namespace KalanMoney.Domain.UseCases.GetAccountDashboard;

public interface IAccountDashboardInput
{
    public void Execute(string ownerId, IAccountDashboardOutput output);
}