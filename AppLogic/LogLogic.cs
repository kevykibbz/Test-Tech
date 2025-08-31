using DataAccess.Repositories;
using ServiceModel;

namespace AppLogic;

public class LogLogic(ILogRepository logRepository) : ILogLogic
{
    private readonly ILogRepository _logRepository = logRepository;

    public async Task<IEnumerable<LogEntry>> GetLogEntriesAsync(LogQueryParameters? parameters = null)
    {
        return await _logRepository.GetLogEntriesAsync(parameters).ConfigureAwait(false);
    }

    public async Task AddLogEntryAsync(LogEntry logEntry)
    {
        await _logRepository.AddLogEntryAsync(logEntry).ConfigureAwait(false);
    }
}
