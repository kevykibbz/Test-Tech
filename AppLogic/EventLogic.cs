using DataAccess.Repositories;
using ServiceModel;

namespace AppLogic;

public class EventLogic(IEventRepository eventRepository) : IEventLogic
{
    private readonly IEventRepository _eventRepository = eventRepository;

    public async Task<IEnumerable<EventType>> GetEventTypesAsync()
    {
        return await _eventRepository.GetEventTypesAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<EventTypeGroup>> GetEventTypeGroupsAsync()
    {
        return await _eventRepository.GetEventTypeGroupsAsync().ConfigureAwait(false);
    }

    public async Task<EventType?> GetEventTypeByIdAsync(string id)
    {
        return await _eventRepository.GetEventTypeByIdAsync(id).ConfigureAwait(false);
    }

    public async Task<EventTypeGroup?> GetEventTypeGroupByIdAsync(string id)
    {
        return await _eventRepository.GetEventTypeGroupByIdAsync(id).ConfigureAwait(false);
    }
}
