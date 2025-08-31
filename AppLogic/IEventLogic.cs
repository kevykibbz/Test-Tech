using ServiceModel;

namespace AppLogic;

public interface IEventLogic
{
    Task<IEnumerable<EventType>> GetEventTypesAsync();
    Task<IEnumerable<EventTypeGroup>> GetEventTypeGroupsAsync();
    Task<EventType?> GetEventTypeByIdAsync(string id);
    Task<EventTypeGroup?> GetEventTypeGroupByIdAsync(string id);
}
