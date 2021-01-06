using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Core.Handler
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;

        public string Input { get; private set; }
        public int NumberOfCalls { get; private set; }

        public MockHttpMessageHandler(string response, HttpStatusCode oK)
        {
            _response = response;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            NumberOfCalls++;  
            if (request.Content != null)
            {
                Input = await request.Content.ReadAsStringAsync();
            }

            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_response)
            };
        }
    }
}
