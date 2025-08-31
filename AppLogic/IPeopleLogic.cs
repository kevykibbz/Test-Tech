using ServiceModel;

namespace AppLogic;

public interface IPeopleLogic
{
    Task<IEnumerable<Person>> GetPeopleAsync();
    Task<Person?> GetPersonByIdAsync(string id);
}
