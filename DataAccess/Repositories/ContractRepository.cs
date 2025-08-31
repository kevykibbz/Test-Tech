namespace DataAccess.Repositories;

public class ContractRepository : IContractRepository
{
    public Task<byte[]> GetSampleContractFileAsync()
    {
        // Get the base directory of the application
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Construct the path to the Resources directory
        var resourcesPath = Path.Combine(baseDirectory, "Resources");

        // Get the contract file path
        var contractFileName = "AlliedEsportsEntertainmentInc_20190815_8-K_EX-10.19_11788293_EX-10.19_Content License Agreement.pdf";
        var contractFilePath = Path.Combine(resourcesPath, contractFileName);

        // Check if the file exists
        if (!File.Exists(contractFilePath))
        {
            throw new FileNotFoundException($"Contract file not found at: {contractFilePath}");
        }

        // Read and return the file as byte array
        return File.ReadAllBytesAsync(contractFilePath);
    }
}
