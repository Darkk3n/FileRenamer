namespace TreasuryToolkit.Core.Contracts
{
    public interface ICompanyService
    {
        IReadOnlyList<string> GetCompanyNames();
    }
}