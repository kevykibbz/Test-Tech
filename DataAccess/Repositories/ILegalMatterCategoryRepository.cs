using ServiceModel;

namespace DataAccess.Repositories;

public interface ILegalMatterCategoryRepository
{
    Task<IEnumerable<LegalMatterCategory>> GetLegalMatterCategoriesAsync();
    Task<LegalMatterCategory?> GetLegalMatterCategoryByIdAsync(string id);
}
