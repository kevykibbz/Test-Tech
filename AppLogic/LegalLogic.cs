using DataAccess.DataModel;
using DataAccess.Repositories;
using ServiceModel;

namespace AppLogic;

public class LegalLogic(ILegalRepository legalRepo, IObjectMapper mapper) : ILegalLogic
{
    private readonly ILegalRepository _legalRepo = legalRepo ?? throw new ArgumentNullException(nameof(legalRepo));
    private readonly IObjectMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<LegalMatter> CreateMatterAsync(LegalMatter legalMatter)
    {
        ArgumentNullException.ThrowIfNull(legalMatter);

        var dbObject = _mapper.Map<LegalMatter, DbLegalMatter>(legalMatter);
        var result = await _legalRepo.CreateLegalMatterAsync(dbObject).ConfigureAwait(false);
        return _mapper.Map<DbLegalMatter, LegalMatter>(result);
    }

    public async Task<LegalMatter> GetMatterAsync(Guid id)
    {
        var result = await _legalRepo.GetLegalMatterAsync(id).ConfigureAwait(false) ?? throw new InvalidOperationException($"LegalMatter with id '{id}' was not found.");
        return _mapper.Map<DbLegalMatter, LegalMatter>(result);
    }

    public async Task<IEnumerable<LegalMatter>> GetMattersAsync(int skip = 0, int take = 100)
    {
        var result = await _legalRepo.GetLegalMattersAsync(skip, take).ConfigureAwait(false);
        return _mapper.Map<DbLegalMatter, LegalMatter>(result);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _legalRepo.GetTotalCountAsync().ConfigureAwait(false);
    }

    public async Task<LegalMatter?> UpdateMatterAsync(LegalMatter legalMatter)
    {
        ArgumentNullException.ThrowIfNull(legalMatter);

        var dbObject = _mapper.Map<LegalMatter, DbLegalMatter>(legalMatter);
        var result = await _legalRepo.UpdateLegalMatterAsync(dbObject).ConfigureAwait(false);
        return result != null ? _mapper.Map<DbLegalMatter, LegalMatter>(result) : null;
    }

    public async Task<bool> DeleteMatterAsync(Guid id)
    {
        return await _legalRepo.DeleteLegalMatterAsync(id).ConfigureAwait(false);
    }

    public async Task<IEnumerable<LegalMatter>> GetMattersByLawyerAsync(Guid lawyerId)
    {
        var result = await _legalRepo.GetLegalMattersByLawyerAsync(lawyerId).ConfigureAwait(false);
        return _mapper.Map<DbLegalMatter, LegalMatter>(result);
    }
}
