using Azure.Messaging.ServiceBus;
using InvoiceProcessor.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace InvoiceProcessor.Infrastructure.Messaging
{
    public class ServiceBusService : IQueueService
    {
        private readonly ServiceBusSender _sender;

        public ServiceBusService(string connectionString, string queueName)
        {
            var client = new ServiceBusClient(connectionString);
            _sender = client.CreateSender(queueName);
        }

        public async Task SendMessageAsync<T>(T message)
        {
            var json = JsonSerializer.Serialize(message);
            var serviceBusMessage = new ServiceBusMessage(json);

            await _sender.SendMessageAsync(serviceBusMessage);
        }
    }
}
