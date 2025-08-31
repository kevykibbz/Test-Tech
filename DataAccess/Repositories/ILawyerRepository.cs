using DataAccess.DataModel;

namespace DataAccess.Repositories;

public interface ILawyerRepository
{
    Task<DbLawyer> CreateLawyerAsync(DbLawyer lawyer);
    Task<DbLawyer?> GetLawyerAsync(Guid id);
    Task<IReadOnlyList<DbLawyer>> GetLawyersAsync(int skip, int take);
    Task<DbLawyer?> UpdateLawyerAsync(DbLawyer lawyer);
    Task<bool> DeleteLawyerAsync(Guid id);
    Task AssignMattersToLawyerAsync(Guid lawyerId, IReadOnlyCollection<Guid> matterIds);
    Task<IReadOnlyList<DbLegalMatter>> GetLawyerMattersAsync(Guid lawyerId);
    Task<DbLawyer?> GetLawyerByMatterAsync(Guid matterId);
}
