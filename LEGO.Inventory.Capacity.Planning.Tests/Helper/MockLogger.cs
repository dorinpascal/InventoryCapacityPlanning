using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace LEGO.Inventory.Capacity.Planning.Tests.Helper;

public static class LoggerExtensions
{
    public static void VerifyLog<T>(this Mock<ILogger<T>> logger, LogLevel logLevel, string expectedMessage, Times times)
    {
        logger.Verify(
            x => x.Log(
                logLevel,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            times);
    }
}
