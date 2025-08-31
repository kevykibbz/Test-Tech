using DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;
using ServiceModel;

namespace DataAccess.Repositories;

public class LegalMatterCategoryRepository(TechTestDbContext context) : ILegalMatterCategoryRepository
{
    private readonly TechTestDbContext _context = context;

    public async Task<IEnumerable<LegalMatterCategory>> GetLegalMatterCategoriesAsync()
    {
        var dbCategories = await _context.LegalMatterCategory.ToListAsync().ConfigureAwait(false);
        return dbCategories.Select(MapToServiceModel);
    }

    public async Task<LegalMatterCategory?> GetLegalMatterCategoryByIdAsync(string id)
    {
        var dbCategory = await _context.LegalMatterCategory
            .FirstOrDefaultAsync(c => c.Id == id)
            .ConfigureAwait(false);
        
        return dbCategory != null ? MapToServiceModel(dbCategory) : null;
    }

    private static LegalMatterCategory MapToServiceModel(DbLegalMatterCategory dbCategory)
    {
        return new LegalMatterCategory
        {
            Id = dbCategory.Id,
            Name = dbCategory.Name
        };
    }
}
