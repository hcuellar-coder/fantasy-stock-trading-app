using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Core.Handler
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public string Input { get; private set; }
        private readonly string _response;

        public MockHttpMessageHandler(string response)
        {
            _response = response;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            if (requestMessage.Content != null)
            {
                Input = await requestMessage.Content.ReadAsStringAsync();
            }

            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_response)
            };
        }
    }
}
