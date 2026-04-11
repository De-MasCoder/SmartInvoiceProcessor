using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessor.Application.Documents.Commands.UploadDocument
{
    public class UploadDocumentCommand : IRequest<Guid>
    {
        public Stream FileStream { get; set; } = default!;
        public string FileName { get; set; } = default!;
    }
}
