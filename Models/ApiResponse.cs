using System.Net;

namespace Contabsv_core.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ApiResModel
    {
        public bool Success { get; set; }
        public object? Data { get; set; } = null;
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
