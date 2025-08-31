using DataAccess.DataModel;
using DataAccess.Repositories;
using ServiceModel;

namespace AppLogic;

public class LawyerLogic(ILawyerRepository lawyerRepo, ILegalRepository legalRepo, IObjectMapper mapper) : ILawyerLogic
{
    private readonly ILawyerRepository _lawyerRepo = lawyerRepo ?? throw new ArgumentNullException(nameof(lawyerRepo));
    private readonly ILegalRepository _legalRepo = legalRepo ?? throw new ArgumentNullException(nameof(legalRepo));
    private readonly IObjectMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<Lawyer> CreateLawyerAsync(Lawyer lawyer)
    {
        ArgumentNullException.ThrowIfNull(lawyer);

        var dbObject = _mapper.Map<Lawyer, DbLawyer>(lawyer);
        var result = await _lawyerRepo.CreateLawyerAsync(dbObject).ConfigureAwait(false);
        return _mapper.Map<DbLawyer, Lawyer>(result);
    }

    public async Task<Lawyer> GetLawyerAsync(Guid id)
    {
        var result = await _lawyerRepo.GetLawyerAsync(id).ConfigureAwait(false) ?? throw new ArgumentException($"Lawyer with ID {id} not found", nameof(id));
        return _mapper.Map<DbLawyer, Lawyer>(result);
    }

    public async Task<IEnumerable<Lawyer>> GetLawyersAsync(int skip = 0, int take = 100)
    {
        var result = await _lawyerRepo.GetLawyersAsync(skip, take).ConfigureAwait(false);
        return _mapper.Map<DbLawyer, Lawyer>(result);
    }

    public async Task<Lawyer?> UpdateLawyerAsync(Lawyer lawyer)
    {
        ArgumentNullException.ThrowIfNull(lawyer);

        var dbObject = _mapper.Map<Lawyer, DbLawyer>(lawyer);
        var result = await _lawyerRepo.UpdateLawyerAsync(dbObject).ConfigureAwait(false);
        return result != null ? _mapper.Map<DbLawyer, Lawyer>(result) : null;
    }

    public async Task<bool> DeleteLawyerAsync(Guid id)
    {
        return await _lawyerRepo.DeleteLawyerAsync(id).ConfigureAwait(false);
    }

    public async Task AssignMattersToLawyerAsync(Guid lawyerId, List<Guid> matterIds)
    {
        ArgumentNullException.ThrowIfNull(matterIds);

        // Verify lawyer exists
        var lawyer = await _lawyerRepo.GetLawyerAsync(lawyerId).ConfigureAwait(false);
        if (lawyer == null)
        {
            throw new ArgumentException($"Lawyer with ID {lawyerId} not found", nameof(lawyerId));
        }

        // Verify all matters exist
        foreach (var matterId in matterIds)
        {
            var matter = await _legalRepo.GetLegalMatterAsync(matterId).ConfigureAwait(false);
            if (matter == null)
            {
                throw new ArgumentException($"Legal matter with ID {matterId} not found", nameof(matterIds));
            }
        }

        await _lawyerRepo.AssignMattersToLawyerAsync(lawyerId, matterIds).ConfigureAwait(false);
    }

    public async Task<IEnumerable<LegalMatter>> GetLawyerMattersAsync(Guid lawyerId)
    {
        var dbMatters = await _lawyerRepo.GetLawyerMattersAsync(lawyerId).ConfigureAwait(false);
        return _mapper.Map<DbLegalMatter, LegalMatter>(dbMatters);
    }
}
