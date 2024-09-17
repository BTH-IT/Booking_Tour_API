using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helper
{
    public class ApiResponse<T>
    {
        public ApiResponse(int statusCode, T result, string message) 
            => (StatusCode, Result, Message) = (statusCode, result, message);
        public int StatusCode { get; set; }
        public T Result { get; set; }
        public string Message { get; set; }
    }
}
