using AppLogic.Contracts;
using AppLogic.Contracts.Models;
using AppLogic.Ollama.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppLogic.Tests.XUnit;

public class ContractExtractionServiceTests
{
    private readonly Mock<IContractService> _mockContractService;
    private readonly Mock<IOllamaClient> _mockOllamaClient;
    private readonly Mock<ILogger<ContractExtractionService>> _mockLogger;
    private readonly ContractExtractionService _service;

    public ContractExtractionServiceTests()
    {
        _mockContractService = new Mock<IContractService>();
        _mockOllamaClient = new Mock<IOllamaClient>();
        _mockLogger = new Mock<ILogger<ContractExtractionService>>();

        _service = new ContractExtractionService(
            _mockContractService.Object,
            _mockOllamaClient.Object,
            _mockLogger.Object
        );
    }

    private static ContractDocument CreatePagedContractText(string text)
    {
        return new ContractDocument
        {
            TotalPages = 1,
            Pages = new List<ContractPage>
            {
                new() {
                    PageNumber = 1,
                    Text = text
                }
            }
        };
    }

    [Fact]
    public async Task ExtractContractInformationAsync_ShouldReturnResult_WhenContractTextIsValid()
    {
        // Arrange
        var contractText = "This is a sample contract between Company A and Company B.";
        var expectedResponse = @"
PARTIES:
Company A
Company B

CONTRACT_TYPE:
Service Agreement

KEY_DATES:
2024-01-01 - Contract start date

FINANCIAL_TERMS:
1000.00 USD - Monthly service fee

KEY_OBLIGATIONS:
Company A shall provide services
Company B shall make payments

TERMINATION_CLAUSES:
Either party may terminate with 30 days notice

INTELLECTUAL_PROPERTY:
None found

CONFIDENTIALITY:
All information is confidential

GOVERNING_LAW:
United States

SUMMARY:
This is a service agreement between Company A and Company B.        ";

        _mockContractService
            .Setup(x => x.RetrieveContractTextAsync())
            .ReturnsAsync(CreatePagedContractText(contractText));

        _mockOllamaClient
            .Setup(x => x.GenerateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _service.ExtractContractInformationAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contractText, result.RawText);
    }

    [Fact]
    public async Task ExtractContractInformationAsync_ShouldThrowException_WhenContractServiceFails()
    {
        // Arrange
        _mockContractService
            .Setup(x => x.RetrieveContractTextAsync())
            .ThrowsAsync(new Exception("Contract service failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ExtractContractInformationAsync());
    }
}
