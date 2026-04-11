using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessor.Domain.Enums
{
    public enum DocumentStatus
    {
        Uploaded = 0,
        Processing = 1,
        Completed = 2,
        Failed = 3
    }
}
