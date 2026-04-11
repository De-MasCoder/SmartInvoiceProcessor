using InvoiceProcessor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessor.Application.Interfaces
{
    public interface IDocumentRepository
    {
        Task AddAsync(Document document);
        Task<Document?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
    }
}
