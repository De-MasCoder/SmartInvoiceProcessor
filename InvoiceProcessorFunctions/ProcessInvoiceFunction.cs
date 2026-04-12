using Azure.Messaging.ServiceBus;
using InvoiceProcessor.Contracts.Events;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using InvoiceProcessorFunctions.Services;

namespace InvoiceProcessorFunctions;

public class ProcessInvoiceFunction
{
    private readonly ILogger<ProcessInvoiceFunction> _logger;
    private readonly InvoiceProcessorService _processor;

    public ProcessInvoiceFunction(
        ILogger<ProcessInvoiceFunction> logger,
        InvoiceProcessorService processor)
    {
        _logger = logger;
        _processor = processor;
    }

    [Function("ProcessInvoice")]
    public async Task Run([ServiceBusTrigger("document-uploads", Connection = "ServiceBusConnection")] 
        ServiceBusReceivedMessage message,
        FunctionContext context)
    {
        var body = message.Body.ToString();

        var documentEvent = JsonSerializer.Deserialize<DocumentUploadedEvent>(body);

        var correlationId = documentEvent?.CorrelationId ?? Guid.NewGuid().ToString();

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId
        }))
        {
            _logger.LogInformation("Processing document {DocumentId}", documentEvent?.DocumentId);

            await _processor.ProcessAsync(documentEvent!);
        }
    }
}