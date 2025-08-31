using ServiceModel;

namespace AppLogic;

public interface ILawyerLogic
{
    Task<Lawyer> CreateLawyerAsync(Lawyer lawyer);
    Task<Lawyer> GetLawyerAsync(Guid id);
    Task<IEnumerable<Lawyer>> GetLawyersAsync(int skip, int take);
    Task<Lawyer?> UpdateLawyerAsync(Lawyer lawyer);
    Task<bool> DeleteLawyerAsync(Guid id);
    Task AssignMattersToLawyerAsync(Guid lawyerId, List<Guid> matterIds);
    Task<IEnumerable<LegalMatter>> GetLawyerMattersAsync(Guid lawyerId);
}
