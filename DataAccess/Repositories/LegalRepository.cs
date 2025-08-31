using DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class LegalRepository : ILegalRepository
{
    private readonly TechTestDbContext _techTestDbContext;

    public LegalRepository(TechTestDbContext techTestDbContext)
    {
        _techTestDbContext = techTestDbContext;
    }

    public async Task<DbLegalMatter> CreateLegalMatterAsync(DbLegalMatter newMatter)
    {
        var id = await _techTestDbContext.AddAsync(newMatter).ConfigureAwait(false);
        await _techTestDbContext.SaveChangesAsync().ConfigureAwait(false);
        return id.Entity;
    }

    public async Task<IReadOnlyList<DbLegalMatter>> GetLegalMattersAsync(int skip = 0, int take = 100) => await _techTestDbContext.Matter
        .Include(m => m.Lawyer)
        .Skip(skip)
        .Take(take)
        .ToListAsync()
        .ConfigureAwait(continueOnCapturedContext: false);

    public Task<DbLegalMatter?> GetLegalMatterAsync(Guid id) => _techTestDbContext.Matter
        .Include(m => m.Lawyer)
        .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<int> GetTotalCountAsync() => await _techTestDbContext.Matter.CountAsync().ConfigureAwait(false);

    public async Task<DbLegalMatter?> UpdateLegalMatterAsync(DbLegalMatter matter)
    {
        ArgumentNullException.ThrowIfNull(matter);
        
        var existing = await _techTestDbContext.Matter.FirstOrDefaultAsync(x => x.Id == matter.Id).ConfigureAwait(false);
        if (existing == null)
        {
            return null;
        }

        // Since DbLegalMatter is a record, we need to replace the entity
        // Remove the existing entity
        _techTestDbContext.Matter.Remove(existing);
        
        // Add the updated matter (preserving the original ID)
        var updatedMatter = new DbLegalMatter(matter.Id, matter.MatterName)
        {
            LawyerId = matter.LawyerId
        };
        
        await _techTestDbContext.Matter.AddAsync(updatedMatter).ConfigureAwait(false);
        await _techTestDbContext.SaveChangesAsync().ConfigureAwait(false);
        return updatedMatter;
    }

    public async Task<bool> DeleteLegalMatterAsync(Guid id)
    {
        var matter = await _techTestDbContext.Matter.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
        if (matter == null)
        {
            return false;
        }

        _techTestDbContext.Matter.Remove(matter);
        await _techTestDbContext.SaveChangesAsync().ConfigureAwait(false);
        return true;
    }

    public async Task<IReadOnlyList<DbLegalMatter>> GetLegalMattersByLawyerAsync(Guid lawyerId)
    {
        return await _techTestDbContext.Matter
            .Include(m => m.Lawyer)
            .Where(m => m.LawyerId == lawyerId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync()
            .ConfigureAwait(false);
    }
}
