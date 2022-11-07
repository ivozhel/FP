using System.Net;

namespace PearlyWhites.Models.Models.Responses
{
    public class BaseResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }       
        public T Respone { get; set; }

    }
}
