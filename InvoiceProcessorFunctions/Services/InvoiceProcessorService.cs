using Azure.Storage.Blobs;
using InvoiceProcessor.Contracts.Events;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Infrastructure.Persistence;
using InvoiceProcessorFunctions.Services.OpenAi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace InvoiceProcessorFunctions.Services
{
    public class InvoiceProcessorService
    {
        private readonly BlobContainerClient _container;
        private readonly AppDbContext _db;
        private readonly ILogger<InvoiceProcessorService> _logger;
        private readonly IOpenAiService _openAiService;

        public InvoiceProcessorService(
            BlobContainerClient container,
            AppDbContext db,
            ILogger<InvoiceProcessorService> logger,
            IOpenAiService openAiService)
        {
            _container = container;
            _db = db;
            _logger = logger;
            _openAiService = openAiService;
        }

        public async Task ProcessAsync(DocumentUploadedEvent message)
        {
            var document = await _db.Documents
                .FirstOrDefaultAsync(x => x.Id == message.DocumentId);

            if (document == null)
            {
                _logger.LogWarning("Document not found {DocumentId}", message.DocumentId);
                return;
            }

            if (document.Status == DocumentStatus.Completed)
            {
                _logger.LogWarning("Already processed {DocumentId}", document.Id);
                return;
            }

            if (document.Status == DocumentStatus.Processing)
            {
                _logger.LogWarning("Already processing {DocumentId}", document.Id);
                return;
            }

            try
            {
                document.MarkAsProcessing();
                await _db.SaveChangesAsync();

                var blobClient = _container.GetBlobClient(message.BlobName);

                var stream = await blobClient.OpenReadAsync();
                var content = await new StreamReader(stream).ReadToEndAsync();

                var invoiceData = await _openAiService.ExtractAsync(content);

                if (invoiceData == null)
                {
                    _logger.LogWarning("Failed to parse invoice data for {DocumentId}", document.Id);
                    document.MarkAsFailed();
                    await _db.SaveChangesAsync();
                    return;
                }

                var extractedJson = JsonSerializer.Serialize(invoiceData);

                document.MarkAsCompleted(extractedJson);

                await _db.SaveChangesAsync();

                _logger.LogInformation("Document processed successfully {DocumentId}", document.Id);
            }
            catch (Exception ex)
            {
                document.MarkAsFailed();
                await _db.SaveChangesAsync();

                _logger.LogError(ex, "Processing failed {DocumentId}", document.Id);

                throw;
            }
        }
    }
}
