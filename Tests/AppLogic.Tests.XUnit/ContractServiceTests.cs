using AppLogic.Contracts;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppLogic.Tests.XUnit;

public class ContractServiceTests
{
    private readonly Mock<IContractRepository> _mockContractRepository;
    private readonly Mock<ILogger<ContractService>> _mockLogger;
    private readonly ContractService _contractService;

    public ContractServiceTests()
    {
        _mockContractRepository = new Mock<IContractRepository>();
        _mockLogger = new Mock<ILogger<ContractService>>();
        _contractService = new ContractService(_mockContractRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task RetrieveContractTextAsync_WithValidPdfBytes_ReturnsExtractedText()
    {
        // Arrange
        // This is a minimal valid PDF with some text content
        var validPdfBytes = CreateMinimalValidPdf();
        _mockContractRepository.Setup(x => x.GetSampleContractFileAsync())
            .ReturnsAsync(validPdfBytes);

        // Act
        var result = await _contractService.RetrieveContractTextAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.TotalPages > 0);
        Assert.True(result.HasText);
        Assert.NotEmpty(result.FullText);
        _mockContractRepository.Verify(x => x.GetSampleContractFileAsync(), Times.Once);
    }

    [Fact]
    public async Task RetrieveContractTextAsync_WithEmptyPdfBytes_ThrowsInvalidOperationException()
    {
        // Arrange
        _mockContractRepository.Setup(x => x.GetSampleContractFileAsync())
            .ReturnsAsync([]);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(_contractService.RetrieveContractTextAsync);

        Assert.Equal("Contract PDF file is empty or null", exception.Message);
    }

    [Fact]
    public async Task RetrieveContractTextAsync_WithNullPdfBytes_ThrowsInvalidOperationException()
    {
        // Arrange
        _mockContractRepository.Setup(x => x.GetSampleContractFileAsync())
            .ReturnsAsync((byte[])null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(_contractService.RetrieveContractTextAsync);

        Assert.Equal("Contract PDF file is empty or null", exception.Message);
    }

    [Fact]
    public async Task RetrieveContractTextAsync_WithInvalidPdfBytes_ThrowsInvalidOperationException()
    {
        // Arrange
        var invalidPdfBytes = "Not a PDF file"u8.ToArray();
        _mockContractRepository.Setup(x => x.GetSampleContractFileAsync())
            .ReturnsAsync(invalidPdfBytes);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(_contractService.RetrieveContractTextAsync);

        Assert.Equal("Failed to retrieve contract text", exception.Message);
    }

    private static byte[] CreateMinimalValidPdf()
    {
        // This creates a minimal PDF structure that iText7 can parse
        // Note: In a real scenario, you would use actual PDF bytes or mock the PDF parsing
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
            (Test Contract) Tj
            ET
            endstream
            endobj
            xref
            0 6
            0000000000 65535 f 
            0000000010 00000 n 
            0000000053 00000 n 
            0000000125 00000 n 
            0000000348 00000 n 
            0000000465 00000 n 
            trailer
            <<
            /Size 6
            /Root 1 0 R
            >>
            startxref
            559
            %%EOF
            """;

        return System.Text.Encoding.UTF8.GetBytes(pdfContent);
    }
}
