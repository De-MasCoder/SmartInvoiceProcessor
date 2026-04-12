using InvoiceProcessor.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceProcessorFunctions.Services.OpenAi
{
    public interface IOpenAiService
    {
        Task<InvoiceData?> ExtractAsync(string content);
    }
}
