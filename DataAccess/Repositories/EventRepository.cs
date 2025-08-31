using DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;
using ServiceModel;

namespace DataAccess.Repositories;

public class EventRepository(TechTestDbContext context) : IEventRepository
{
    private readonly TechTestDbContext _context = context;

    public async Task<IEnumerable<EventType>> GetEventTypesAsync()
    {
        var dbEventTypes = await _context.EventType
            .Include(e => e.Group)
            .ToListAsync()
            .ConfigureAwait(false);
        
        return dbEventTypes.Select(MapEventTypeToServiceModel);
    }

    public async Task<IEnumerable<EventTypeGroup>> GetEventTypeGroupsAsync()
    {
        var dbEventTypeGroups = await _context.EventTypeGroup
            .ToListAsync()
            .ConfigureAwait(false);
        
        return dbEventTypeGroups.Select(MapEventTypeGroupToServiceModel);
    }

    public async Task<EventType?> GetEventTypeByIdAsync(string id)
    {
        var dbEventType = await _context.EventType
            .Include(e => e.Group)
            .FirstOrDefaultAsync(e => e.Id == id)
            .ConfigureAwait(false);
        
        return dbEventType != null ? MapEventTypeToServiceModel(dbEventType) : null;
    }

    public async Task<EventTypeGroup?> GetEventTypeGroupByIdAsync(string id)
    {
        var dbEventTypeGroup = await _context.EventTypeGroup
            .FirstOrDefaultAsync(g => g.Id == id)
            .ConfigureAwait(false);
        
        return dbEventTypeGroup != null ? MapEventTypeGroupToServiceModel(dbEventTypeGroup) : null;
    }

    private static EventType MapEventTypeToServiceModel(DbEventType dbEventType)
    {
        return new EventType
        {
            Id = dbEventType.Id,
            Name = dbEventType.Name,
            Description = dbEventType.Description,
            GroupId = dbEventType.GroupId
        };
    }

    private static EventTypeGroup MapEventTypeGroupToServiceModel(DbEventTypeGroup dbEventTypeGroup)
    {
        return new EventTypeGroup
        {
            Id = dbEventTypeGroup.Id,
            Name = dbEventTypeGroup.Name,
            Description = dbEventTypeGroup.Description
        };
    }
}
