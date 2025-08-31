using DataAccess.Repositories;
using ServiceModel;

namespace AppLogic;

public class LegalMatterCategoryLogic(ILegalMatterCategoryRepository categoryRepository) : ILegalMatterCategoryLogic
{
    private readonly ILegalMatterCategoryRepository _categoryRepository = categoryRepository;

    public async Task<IEnumerable<LegalMatterCategory>> GetLegalMatterCategoriesAsync()
    {
        return await _categoryRepository.GetLegalMatterCategoriesAsync().ConfigureAwait(false);
    }

    public async Task<LegalMatterCategory?> GetLegalMatterCategoryByIdAsync(string id)
    {
        return await _categoryRepository.GetLegalMatterCategoryByIdAsync(id).ConfigureAwait(false);
    }
}
