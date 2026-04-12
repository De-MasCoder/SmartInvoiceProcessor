using InvoiceProcessor.Application.Interfaces;
using InvoiceProcessor.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace InvoiceProcessor.Infrastructure.Persistence
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly AppDbContext _context;

        public DocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Document document, CancellationToken cancellationToken)
        {
            await _context.Documents.AddAsync(document,cancellationToken);
        }

        public async Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Documents.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
