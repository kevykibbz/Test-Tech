using AppLogic.Contracts.Models;
using DataAccess.Repositories;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.Extensions.Logging;

namespace AppLogic.Contracts;

public class ContractService(IContractRepository contractRepository, ILogger<ContractService> logger) : IContractService
{
    public async Task<ContractDocument> RetrieveContractTextAsync()
    {
        try
        {
            logger.LogInformation("Starting to retrieve and parse contract text from PDF");

            var pdfBytes = await contractRepository.GetSampleContractFileAsync();

            if (pdfBytes == null || pdfBytes.Length == 0)
            {
                throw new InvalidOperationException("Contract PDF file is empty or null");
            }

            var pagedText = ExtractPagedTextFromPdf(pdfBytes);

            logger.LogInformation("Successfully extracted text from PDF, size: {PdfSize} bytes, total pages: {TotalPages}, total text length: {TextLength} characters", pdfBytes.Length, pagedText.TotalPages, pagedText.TotalCharacterCount);

            return pagedText;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve contract text from PDF");
            throw new InvalidOperationException("Failed to retrieve contract text", ex);
        }
    }

    private ContractDocument ExtractPagedTextFromPdf(byte[] pdfBytes)
    {
        try
        {
            using var memoryStream = new MemoryStream(pdfBytes);
            using var pdfReader = new PdfReader(memoryStream);
            using var pdfDocument = new PdfDocument(pdfReader);

            var numberOfPages = pdfDocument.GetNumberOfPages();
            var pagedText = new ContractDocument
            {
                TotalPages = numberOfPages
            };

            logger.LogInformation("Processing PDF with {PageCount} pages", numberOfPages);

            for (int pageNumber = 1; pageNumber <= numberOfPages; pageNumber++)
            {
                var page = pdfDocument.GetPage(pageNumber);
                var strategy = new SimpleTextExtractionStrategy();
                var pageText = PdfTextExtractor.GetTextFromPage(page, strategy);

                var contractPage = new ContractPage
                {
                    PageNumber = pageNumber,
                    Text = pageText?.Trim() ?? string.Empty
                };

                pagedText.Pages.Add(contractPage);

                logger.LogDebug("Extracted text from page {PageNumber}, characters: {CharacterCount}", pageNumber, contractPage.CharacterCount);
            }

            if (!pagedText.HasText)
            {
                throw new InvalidOperationException("No text could be extracted from the PDF");
            }

            return pagedText;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to extract text from PDF bytes");
            throw new InvalidOperationException("Failed to parse PDF content", ex);
        }
    }
}
