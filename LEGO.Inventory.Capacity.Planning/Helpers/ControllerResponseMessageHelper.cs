using LEGO.Inventory.Capacity.Planning.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Authentication;

namespace LEGO.Inventory.Capacity.Planning.Helpers;

public static class ControllerResponseMessageHelper
{
    public static ObjectResult HandleException(Exception exception, ILogger logger)
    {
        var (statusCode, message, errors) = exception switch
        {
            ArgumentNullException notFoundException =>
                (HttpStatusCode.NotFound, notFoundException.Message, null),
            FormatException formatException =>
                 (HttpStatusCode.BadRequest, formatException.Message, null),
            ArgumentException argException =>
                (HttpStatusCode.BadRequest, argException.Message, null),
            AuthenticationException authenticationException =>
                (HttpStatusCode.Unauthorized, authenticationException.Message, null),
            UnauthorizedAccessException unauthorizedException =>
                (HttpStatusCode.Forbidden, unauthorizedException.Message, null),
            _ =>
                (HttpStatusCode.ServiceUnavailable, "Service Unavailable", new List<string> { exception.Message })
        };

        logger.Log(statusCode == HttpStatusCode.ServiceUnavailable ? LogLevel.Error : LogLevel.Information, message);
        var responseBody = new ErrorResponseBody
        {
            Message = message,
            Errors = errors ?? []
        };

        return new ObjectResult(responseBody) { StatusCode = (int)statusCode };
    }
}
