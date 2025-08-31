using DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;
using ServiceModel;

namespace DataAccess.Repositories;

public class CurrencyRepository(TechTestDbContext context) : ICurrencyRepository
{
    private readonly TechTestDbContext _context = context;

    public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
    {
        var dbCurrencies = await _context.Currency.ToListAsync().ConfigureAwait(false);
        return dbCurrencies.Select(MapToServiceModel);
    }

    public async Task<Currency?> GetCurrencyByIdAsync(string id)
    {
        var dbCurrency = await _context.Currency
            .FirstOrDefaultAsync(c => c.Id == id)
            .ConfigureAwait(false);
        
        return dbCurrency != null ? MapToServiceModel(dbCurrency) : null;
    }

    private static Currency MapToServiceModel(DbCurrency dbCurrency)
    {
        return new Currency
        {
            Id = dbCurrency.Id,
            Symbol = dbCurrency.Symbol,
            Name = dbCurrency.Name,
            DecimalDigits = dbCurrency.DecimalDigits
        };
    }
}
