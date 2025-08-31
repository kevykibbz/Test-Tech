using DataAccess.DataModel;

namespace DataAccess.Repositories;

public interface ILegalRepository
{
    public Task<DbLegalMatter> CreateLegalMatterAsync(DbLegalMatter newMatter);
    public Task<DbLegalMatter?> GetLegalMatterAsync(Guid id);
    public Task<IReadOnlyList<DbLegalMatter>> GetLegalMattersAsync(int skip, int take);
    public Task<int> GetTotalCountAsync();
    public Task<DbLegalMatter?> UpdateLegalMatterAsync(DbLegalMatter matter);
    public Task<bool> DeleteLegalMatterAsync(Guid id);
    public Task<IReadOnlyList<DbLegalMatter>> GetLegalMattersByLawyerAsync(Guid lawyerId);
}
