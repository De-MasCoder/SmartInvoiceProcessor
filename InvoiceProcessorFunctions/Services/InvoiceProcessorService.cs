using Azure.Storage.Blobs;
using InvoiceProcessor.Contracts.Events;
using InvoiceProcessor.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessorFunctions.Services
{
    public class InvoiceProcessorService
    {
        private readonly BlobContainerClient _container;
        private readonly AppDbContext _db;
        private readonly ILogger<InvoiceProcessorService> _logger;

        public InvoiceProcessorService(
            BlobContainerClient container,
            AppDbContext db,
            ILogger<InvoiceProcessorService> logger)
        {
            _container = container;
            _db = db;
            _logger = logger;
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

            try
            {
                document.MarkAsProcessing();
                await _db.SaveChangesAsync();

                var blobClient = _container.GetBlobClient(message.BlobName);

                var stream = await blobClient.OpenReadAsync();

                //  Simulate extraction - Todo : Use AI later
                var extractedData = $"Processed at {DateTime.UtcNow}";

                document.MarkAsCompleted(extractedData);

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
