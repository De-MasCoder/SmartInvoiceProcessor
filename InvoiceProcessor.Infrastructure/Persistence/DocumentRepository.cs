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

        public async Task AddAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
        }

        public async Task<Document?> GetByIdAsync(Guid id)
        {
            return await _context.Documents.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
