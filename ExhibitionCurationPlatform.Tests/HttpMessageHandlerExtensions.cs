using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq.Language.Flow;

namespace ExhibitionCurationPlatform.Tests
{
    public static class HttpMessageHandlerExtensions
    {
        public static ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupRequest(
     this Mock<HttpMessageHandler> mock, HttpMethod method, string url)
        {
            return mock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == method && req.RequestUri!.ToString() == url),
                    ItExpr.IsAny<CancellationToken>());
        }

      public static void ReturnsJson(
      this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup, string json)
        {
            setup.ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        }
    }
}
