using Azure.Storage.Blobs;
using InvoiceProcessor.Infrastructure.Persistence;
using InvoiceProcessorFunctions.Services;
using InvoiceProcessorFunctions.Services.OpenAi;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenAI.Chat;

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

// OpenAI Client
builder.Services.AddSingleton(sp =>
{
    var apiKey = config["OpenAI:ApiKey"];
    return new ChatClient("gpt-4.1-mini", apiKey);
});
builder.Services.AddScoped<IOpenAiService, OpenAiService>();

// Build & run
builder.Build().Run();