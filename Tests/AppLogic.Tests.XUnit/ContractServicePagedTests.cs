using AppLogic.Contracts;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppLogic.Tests.XUnit;

/// <summary>
/// Tests for the refactored ContractService with PagedContractText
/// </summary>
public class ContractServicePagedTests
{
    private readonly Mock<IContractRepository> _mockContractRepository;
    private readonly Mock<ILogger<ContractService>> _mockLogger;
    private readonly ContractService _contractService;

    public ContractServicePagedTests()
    {
        _mockContractRepository = new Mock<IContractRepository>();
        _mockLogger = new Mock<ILogger<ContractService>>();
        _contractService = new ContractService(_mockContractRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task RetrieveContractTextAsync_WithValidPdfBytes_ReturnsPagedContractText()
    {
        // Arrange
        // This is a minimal valid PDF with some text content
        var validPdfBytes = CreateMinimalValidPdf();
        _mockContractRepository.Setup(x => x.GetSampleContractFileAsync()).ReturnsAsync(validPdfBytes);

        // Act
        var result = await _contractService.RetrieveContractTextAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.TotalPages > 0);
        Assert.True(result.HasText);
        Assert.NotEmpty(result.FullText);
        Assert.NotEmpty(result.Pages);
        Assert.True(result.TotalCharacterCount > 0);

        // Check that page numbering is correct
        for (int i = 0; i < result.Pages.Count; i++)
        {
            Assert.Equal(i + 1, result.Pages[i].PageNumber);
        }

        _mockContractRepository.Verify(x => x.GetSampleContractFileAsync(), Times.Once);
    }

    [Fact]
    public async Task RetrieveContractTextAsync_WithEmptyPdfBytes_ThrowsInvalidOperationException()
    {
        // Arrange
        _mockContractRepository.Setup(x => x.GetSampleContractFileAsync()).ReturnsAsync([]);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(_contractService.RetrieveContractTextAsync);

        Assert.Equal("Contract PDF file is empty or null", exception.Message);
    }

    [Fact]
    public async Task RetrieveContractTextAsync_WithNullPdfBytes_ThrowsInvalidOperationException()
    {
        // Arrange
        _mockContractRepository.Setup(x => x.GetSampleContractFileAsync()).ReturnsAsync((byte[])null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(_contractService.RetrieveContractTextAsync);

        Assert.Equal("Contract PDF file is empty or null", exception.Message);
    }

    [Fact]
    public async Task RetrieveContractTextAsync_WithInvalidPdfBytes_ThrowsInvalidOperationException()
    {
        // Arrange
        var invalidPdfBytes = "Not a PDF file"u8.ToArray();
        _mockContractRepository.Setup(x => x.GetSampleContractFileAsync()).ReturnsAsync(invalidPdfBytes);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(_contractService.RetrieveContractTextAsync);

        Assert.Equal("Failed to retrieve contract text", exception.Message);
    }

    private static byte[] CreateMinimalValidPdf()
    {
        // This creates a minimal PDF structure that iText7 can parse
        var pdfContent = """
            %PDF-1.4
            1 0 obj
            <<
            /Type /Catalog
            /Pages 2 0 R
            >>
            endobj
            2 0 obj
            <<
            /Type /Pages
            /Kids [3 0 R]
            /Count 1
            >>
            endobj
            3 0 obj
            <<
            /Type /Page
            /Parent 2 0 R
            /MediaBox [0 0 612 792]
            /Resources <<
            /Font <<
            /F1 4 0 R
            >>
            >>
            /Contents 5 0 R
            >>
            endobj
            4 0 obj
            <<
            /Type /Font
            /Subtype /Type1
            /BaseFont /Helvetica
            >>
            endobj
            5 0 obj
            <<
            /Length 44
            >>
            stream
            BT
            /F1 12 Tf
            100 700 Td
            (Test contract text) Tj
            ET
            endstream
            endobj
            xref
            0 6
            0000000000 65535 f 
            0000000015 00000 n 
            0000000068 00000 n 
            0000000125 00000 n 
            0000000262 00000 n 
            0000000333 00000 n 
            trailer
            <<
            /Size 6
            /Root 1 0 R
            >>
            startxref
            421
            %%EOF
            """;

        return System.Text.Encoding.UTF8.GetBytes(pdfContent);
    }
}
