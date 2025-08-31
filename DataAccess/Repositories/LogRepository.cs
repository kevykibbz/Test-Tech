using DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;
using ServiceModel;

namespace DataAccess.Repositories;

public class LogRepository(TechTestDbContext context) : ILogRepository
{
    private readonly TechTestDbContext _context = context;

    public async Task<IEnumerable<LogEntry>> GetLogEntriesAsync(LogQueryParameters? parameters = null)
    {
        var query = _context.LogEntry.AsQueryable();

        // Apply entity filter
        if (!string.IsNullOrEmpty(parameters?.EntityId))
        {
            query = query.Where(l => l.EntityId == parameters.EntityId);
        }

        // Apply before filter (for pagination)
        if (parameters?.Before.HasValue == true)
        {
            query = query.Where(l => l.Id < parameters.Before.Value);
        }

        // Apply after filter (for real-time updates)
        if (parameters?.After.HasValue == true)
        {
            query = query.Where(l => l.Id > parameters.After.Value);
        }

        // Order by ID descending (newest first)
        query = query.OrderByDescending(l => l.Id);

        // Apply take limit
        if (parameters?.Take.HasValue == true)
        {
            query = query.Take(parameters.Take.Value);
        }

        var dbLogEntries = await query.ToListAsync().ConfigureAwait(false);
        return dbLogEntries.Select(MapToServiceModel);
    }

    public async Task AddLogEntryAsync(LogEntry logEntry)
    {
        var dbLogEntry = new DbLogEntry
        {
            Details = logEntry.Details,
            EntityId = logEntry.EntityId,
            TypeId = logEntry.TypeId,
            CreatedAt = logEntry.CreatedAt ?? DateTime.UtcNow
        };

        _context.LogEntry.Add(dbLogEntry);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    private static LogEntry MapToServiceModel(DbLogEntry dbLogEntry)
    {
        return new LogEntry
        {
            Id = dbLogEntry.Id,
            Details = dbLogEntry.Details,
            EntityId = dbLogEntry.EntityId,
            TypeId = dbLogEntry.TypeId,
            CreatedAt = dbLogEntry.CreatedAt
        };
    }
}
