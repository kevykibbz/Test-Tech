using ServiceModel;

namespace DataAccess.Repositories;

public interface ICurrencyRepository
{
    Task<IEnumerable<Currency>> GetCurrenciesAsync();
    Task<Currency?> GetCurrencyByIdAsync(string id);
}
