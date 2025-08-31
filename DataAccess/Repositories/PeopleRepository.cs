using DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;
using ServiceModel;

namespace DataAccess.Repositories;

public class PeopleRepository(TechTestDbContext context) : IPeopleRepository
{
    private readonly TechTestDbContext _context = context;

    public async Task<IEnumerable<Person>> GetPeopleAsync()
    {
        var dbPeople = await _context.Person.ToListAsync().ConfigureAwait(false);
        return dbPeople.Select(MapToServiceModel);
    }

    public async Task<Person?> GetPersonByIdAsync(string id)
    {
        var dbPerson = await _context.Person
            .FirstOrDefaultAsync(p => p.Id == id)
            .ConfigureAwait(false);
        
        return dbPerson != null ? MapToServiceModel(dbPerson) : null;
    }

    private static Person MapToServiceModel(DbPerson dbPerson)
    {
        return new Person
        {
            Id = dbPerson.Id,
            FirstName = dbPerson.FirstName,
            LastName = dbPerson.LastName,
            FullName = dbPerson.FullName,
            Initials = dbPerson.Initials,
            HasPicture = dbPerson.HasPicture,
            PictureUrl = dbPerson.PictureUrl
        };
    }
}
