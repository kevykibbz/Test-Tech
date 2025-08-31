using ServiceModel;

namespace DataAccess.Repositories;

public interface IPeopleRepository
{
    Task<IEnumerable<Person>> GetPeopleAsync();
    Task<Person?> GetPersonByIdAsync(string id);
}
