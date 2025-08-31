using ServiceModel;

namespace DataAccess.Repositories;

public interface ILogRepository
{
    Task<IEnumerable<LogEntry>> GetLogEntriesAsync(LogQueryParameters? parameters = null);
    Task AddLogEntryAsync(LogEntry logEntry);
}
