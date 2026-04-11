using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessor.Application.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(Stream file, string fileName);
    }
}
