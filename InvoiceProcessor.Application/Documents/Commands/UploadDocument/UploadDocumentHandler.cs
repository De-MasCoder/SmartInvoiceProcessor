using InvoiceProcessor.Application.Events;
using InvoiceProcessor.Application.Interfaces;
using InvoiceProcessor.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessor.Application.Documents.Commands.UploadDocument
{
    public class UploadDocumentHandler : IRequestHandler<UploadDocumentCommand, Guid>
    {
        private readonly IBlobStorageService _blobService;
        private readonly IQueueService _queueService;
        private readonly IDocumentRepository _documentRepository;

        public UploadDocumentHandler(
            IBlobStorageService blobService,
            IQueueService queueService,
            IDocumentRepository documentRepository)
        {
            _blobService = blobService;
            _queueService = queueService;
            _documentRepository = documentRepository;
        }

        public async Task<Guid> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            // Upload to blob
            var blobName = await _blobService.UploadAsync(
                request.FileStream,
                request.FileName);

            // Create domain entity
            var document = new Document(request.FileName, blobName);

            // Save to DB
            await _documentRepository.AddAsync(document);
            await _documentRepository.SaveChangesAsync();

            // Publish event
            var message = new DocumentUploadedEvent
            {
                DocumentId = document.Id,
                BlobName = blobName
            };

            await _queueService.SendMessageAsync(message);

            return document.Id;
        }
    }
}
