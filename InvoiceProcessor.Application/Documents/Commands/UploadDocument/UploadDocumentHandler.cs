using InvoiceProcessor.Application.Interfaces;
using InvoiceProcessor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using InvoiceProcessor.Contracts.Events;

namespace InvoiceProcessor.Application.Documents.Commands.UploadDocument
{
    public class UploadDocumentHandler : IRequestHandler<UploadDocumentCommand, Guid>
    {
        private readonly IBlobStorageService _blobService;
        private readonly IQueueService _queueService;
        private readonly IDocumentRepository _documentRepository;
        private readonly ILogger<UploadDocumentHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadDocumentHandler(
            IBlobStorageService blobService,
            IQueueService queueService,
            IDocumentRepository documentRepository,
            ILogger<UploadDocumentHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _blobService = blobService;
            _queueService = queueService;
            _documentRepository = documentRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {

            var correlationId = _httpContextAccessor.HttpContext?.TraceIdentifier;

            _logger.LogInformation(
                "Uploading document {FileName} with CorrelationId {CorrelationId}",
                request.FileName,
                correlationId);

            // Upload to blob
            var blobName = await _blobService.UploadAsync(
                request.FileStream,
                request.FileName,
                cancellationToken);

            // Create domain entity
            var document = new Document(request.FileName, blobName);

            // Save to DB
            await _documentRepository.AddAsync(document,cancellationToken);
            await _documentRepository.SaveChangesAsync(cancellationToken);

            // Publish event
            var message = new DocumentUploadedEvent
            {
                DocumentId = document.Id,
                BlobName = blobName,
                CorrelationId = correlationId ?? string.Empty
            };

            await _queueService.SendMessageAsync(message, cancellationToken);

            return document.Id;
        }
    }
}
