using DataAccess.Repositories;
using ServiceModel;

namespace AppLogic;

public class PeopleLogic(IPeopleRepository peopleRepository) : IPeopleLogic
{
    private readonly IPeopleRepository _peopleRepository = peopleRepository;

    public async Task<IEnumerable<Person>> GetPeopleAsync()
    {
        return await _peopleRepository.GetPeopleAsync().ConfigureAwait(false);
    }

    public async Task<Person?> GetPersonByIdAsync(string id)
    {
        return await _peopleRepository.GetPersonByIdAsync(id).ConfigureAwait(false);
    }
}
