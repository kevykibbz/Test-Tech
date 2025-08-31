using DataAccess.Repositories;
using ServiceModel;

namespace AppLogic;

public class CurrencyLogic(ICurrencyRepository currencyRepository) : ICurrencyLogic
{
    private readonly ICurrencyRepository _currencyRepository = currencyRepository;

    public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
    {
        return await _currencyRepository.GetCurrenciesAsync().ConfigureAwait(false);
    }

    public async Task<Currency?> GetCurrencyByIdAsync(string id)
    {
        return await _currencyRepository.GetCurrencyByIdAsync(id).ConfigureAwait(false);
    }
}
