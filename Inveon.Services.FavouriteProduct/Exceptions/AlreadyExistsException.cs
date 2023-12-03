using System.Net;

namespace Inveon.Services.FavouriteProduct.Exceptions;

public class AlreadyExitsException : Exception
{
    public HttpStatusCode StatusCode { get; }
    
    public AlreadyExitsException() : base()
    {
    }

    public AlreadyExitsException(string message) : base(message)
    {
    }

    public AlreadyExitsException(string message, Exception innerException) : base(message, innerException)
    {
    }
    public AlreadyExitsException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}