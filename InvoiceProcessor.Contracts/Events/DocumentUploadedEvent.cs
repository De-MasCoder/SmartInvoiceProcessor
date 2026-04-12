using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessor.Contracts.Events
{
    public class DocumentUploadedEvent
    {
        public Guid DocumentId { get; set; }
        public string BlobName { get; set; } = default!;
        public string CorrelationId { get; set; } = default!; 
    }
}
