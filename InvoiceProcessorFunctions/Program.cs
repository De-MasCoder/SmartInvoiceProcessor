using Azure.Storage.Blobs;
using InvoiceProcessor.Infrastructure.Persistence;
using InvoiceProcessorFunctions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

var config = builder.Configuration;

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

// Blob
builder.Services.AddSingleton(_ =>
    new BlobContainerClient(
        config["Blob:ConnectionString"],
        config["Blob:ContainerName"]));

// Processor
builder.Services.AddScoped<InvoiceProcessorService>();

// Build & run
builder.Build().Run();