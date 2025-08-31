using ServiceModel;

namespace AppLogic;

public interface ILegalMatterCategoryLogic
{
    Task<IEnumerable<LegalMatterCategory>> GetLegalMatterCategoriesAsync();
    Task<LegalMatterCategory?> GetLegalMatterCategoryByIdAsync(string id);
}
