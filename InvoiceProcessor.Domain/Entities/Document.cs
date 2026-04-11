using InvoiceProcessor.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessor.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; private set; }

        public string FileName { get; private set; }

        public string BlobName { get; private set; }

        public DocumentStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public string? ExtractedData { get; private set; }

        // Constructor
        public Document(string fileName, string blobName)
        {
            Id = Guid.NewGuid();
            FileName = fileName;
            BlobName = blobName;
            Status = DocumentStatus.Uploaded;
            CreatedAt = DateTime.UtcNow;
        }

        // Update status
        public void MarkAsProcessing()
        {
            Status = DocumentStatus.Processing;
        }

        public void MarkAsCompleted(string extractedData)
        {
            Status = DocumentStatus.Completed;
            ExtractedData = extractedData;
        }

        public void MarkAsFailed()
        {
            Status = DocumentStatus.Failed;
        }
    }
}
