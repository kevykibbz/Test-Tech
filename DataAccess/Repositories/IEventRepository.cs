using ServiceModel;

namespace DataAccess.Repositories;

public interface IEventRepository
{
    Task<IEnumerable<EventType>> GetEventTypesAsync();
    Task<IEnumerable<EventTypeGroup>> GetEventTypeGroupsAsync();
    Task<EventType?> GetEventTypeByIdAsync(string id);
    Task<EventTypeGroup?> GetEventTypeGroupByIdAsync(string id);
}
