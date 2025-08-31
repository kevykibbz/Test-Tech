using ServiceModel;

namespace AppLogic;

public interface ILogLogic
{
    Task<IEnumerable<LogEntry>> GetLogEntriesAsync(LogQueryParameters? parameters = null);
    Task AddLogEntryAsync(LogEntry logEntry);
}
