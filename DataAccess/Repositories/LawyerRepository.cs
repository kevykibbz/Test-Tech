using DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class LawyerRepository : ILawyerRepository
{
    private readonly TechTestDbContext _techTestDbContext;

    public LawyerRepository(TechTestDbContext techTestDbContext)
    {
        _techTestDbContext = techTestDbContext;
    }

    public async Task<DbLawyer> CreateLawyerAsync(DbLawyer lawyer)
    {
        var entity = await _techTestDbContext.AddAsync(lawyer).ConfigureAwait(false);
        await _techTestDbContext.SaveChangesAsync().ConfigureAwait(false);
        return entity.Entity;
    }

    public async Task<IReadOnlyList<DbLawyer>> GetLawyersAsync(int skip = 0, int take = 100) => 
        await _techTestDbContext.Lawyer
            .Skip(skip)
            .Take(take)
            .ToListAsync()
            .ConfigureAwait(false);

    public Task<DbLawyer?> GetLawyerAsync(Guid id) => 
        _techTestDbContext.Lawyer.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<DbLawyer?> UpdateLawyerAsync(DbLawyer lawyer)
    {
        ArgumentNullException.ThrowIfNull(lawyer);

        var existing = await _techTestDbContext.Lawyer
            .FirstOrDefaultAsync(x => x.Id == lawyer.Id)
            .ConfigureAwait(false);

        if (existing == null)
            return null;

        // Remove the old entity and add the updated one
        _techTestDbContext.Lawyer.Remove(existing);
        var updatedLawyer = new DbLawyer(
            lawyer.Id,
            lawyer.FirstName,
            lawyer.LastName,
            lawyer.CompanyName
        );
        
        var entity = await _techTestDbContext.Lawyer.AddAsync(updatedLawyer).ConfigureAwait(false);
        await _techTestDbContext.SaveChangesAsync().ConfigureAwait(false);
        return entity.Entity;
    }

    public async Task<bool> DeleteLawyerAsync(Guid id)
    {
        var lawyer = await _techTestDbContext.Lawyer
            .FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false);

        if (lawyer == null)
            return false;

        // Unassign any matters from this lawyer before deleting
        var matters = await _techTestDbContext.Matter
            .Where(m => m.LawyerId == id)
            .ToListAsync()
            .ConfigureAwait(false);

        foreach (var matter in matters)
        {
            matter.LawyerId = null;
        }

        _techTestDbContext.Lawyer.Remove(lawyer);
        await _techTestDbContext.SaveChangesAsync().ConfigureAwait(false);
        return true;
    }

    public async Task AssignMattersToLawyerAsync(Guid lawyerId, IReadOnlyCollection<Guid> matterIds)
    {
        // Update all matters to reference the lawyer
        var matters = await _techTestDbContext.Matter
            .Where(m => matterIds.Contains(m.Id))
            .ToListAsync()
            .ConfigureAwait(false);

        foreach (var matter in matters)
        {
            matter.LawyerId = lawyerId;
        }

        await _techTestDbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<DbLegalMatter>> GetLawyerMattersAsync(Guid lawyerId) =>
        await _techTestDbContext.Matter
            .Include(m => m.Lawyer)
            .Where(m => m.LawyerId == lawyerId)
            .ToListAsync()
            .ConfigureAwait(false);

    public Task<DbLawyer?> GetLawyerByMatterAsync(Guid matterId) =>
        _techTestDbContext.Matter
            .Where(m => m.Id == matterId && m.LawyerId != null)
            .Join(_techTestDbContext.Lawyer,
                matter => matter.LawyerId,
                lawyer => lawyer.Id,
                (matter, lawyer) => lawyer)
            .FirstOrDefaultAsync();
}
