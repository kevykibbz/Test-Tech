using AppLogic.Contracts.Models;

namespace AppLogic.Contracts;

public interface IContractService
{
    /// <summary>
    /// Retrieves the contract text from the PDF file organized by pages
    /// </summary>
    /// <returns>The extracted text content from the contract PDF, organized by pages</returns>
    Task<ContractDocument> RetrieveContractTextAsync();
}
