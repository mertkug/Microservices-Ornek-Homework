using System.Net;

namespace Inveon.Services.ProductAPI.Exceptions;

public class NotFoundException : Exception
{
    public HttpStatusCode StatusCode { get; }
    
    public NotFoundException() : base()
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
    public NotFoundException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}
