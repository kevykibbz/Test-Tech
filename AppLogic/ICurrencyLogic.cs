using ServiceModel;

namespace AppLogic;

public interface ICurrencyLogic
{
    Task<IEnumerable<Currency>> GetCurrenciesAsync();
    Task<Currency?> GetCurrencyByIdAsync(string id);
}
