using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessor.Application.Interfaces
{
    public interface IQueueService
    {
        Task SendMessageAsync<T>(T message, CancellationToken cancellationToken);
    }
}
