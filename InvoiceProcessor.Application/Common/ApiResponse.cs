using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessor.Application.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? TraceId { get; set; }

        public static ApiResponse<T> Ok(T data, string? traceId = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                TraceId = traceId
            };
        }
    }
}
