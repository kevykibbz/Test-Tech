using ServiceModel;

namespace AppLogic;

public interface ILegalLogic
{
    Task<LegalMatter> CreateMatterAsync(LegalMatter legalMatter);
    Task<LegalMatter> GetMatterAsync(Guid id);
    Task<IEnumerable<LegalMatter>> GetMattersAsync(int skip, int take);
    Task<int> GetTotalCountAsync();
    Task<LegalMatter?> UpdateMatterAsync(LegalMatter legalMatter);
    Task<bool> DeleteMatterAsync(Guid id);
    Task<IEnumerable<LegalMatter>> GetMattersByLawyerAsync(Guid lawyerId);
}
