
namespace DataAccess.Repositories;
public interface IContractRepository
{
    Task<byte[]> GetSampleContractFileAsync();
}
