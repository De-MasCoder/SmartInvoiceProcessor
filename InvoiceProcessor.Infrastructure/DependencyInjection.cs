using InvoiceProcessor.Application.Interfaces;
using InvoiceProcessor.Infrastructure.Messaging;
using InvoiceProcessor.Infrastructure.Persistence;
using InvoiceProcessor.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace InvoiceProcessor.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IBlobStorageService>(sp =>
                new BlobStorageService(
                    configuration["Blob:ConnectionString"],
                    configuration["Blob:ContainerName"]));

            services.AddScoped<IQueueService>(sp =>
                new ServiceBusService(
                    configuration["ServiceBus:ConnectionString"],
                    configuration["ServiceBus:QueueName"]));

            services.AddScoped<IDocumentRepository, DocumentRepository>();

            return services;
        }
    }
}
