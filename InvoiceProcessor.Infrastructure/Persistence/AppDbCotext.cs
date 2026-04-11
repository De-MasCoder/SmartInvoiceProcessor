using InvoiceProcessor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessor.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Document> Documents => Set<Document>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Document>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.FileName).IsRequired();
                entity.Property(x => x.BlobName).IsRequired();
            });
        }
    }
}
