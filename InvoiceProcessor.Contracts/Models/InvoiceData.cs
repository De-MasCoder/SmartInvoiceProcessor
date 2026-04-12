using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessor.Contracts.Models
{
    public class InvoiceData
    {
        public string Vendor { get; set; } = default!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
