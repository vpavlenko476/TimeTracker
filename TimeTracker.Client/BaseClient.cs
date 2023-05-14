using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimeTracker.Client
{
    public abstract class BaseClient : IDisposable
    {
        private readonly HttpClient client; 
        protected BaseClient(Uri baseUrl)
        {
            client = new HttpClient
            {
                BaseAddress = baseUrl,
            };
        }

        protected async Task<TResponse> Get<TResponse>(string url, CancellationToken cancellationToken)
        {
            var response = await client.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }
        
        public async Task PostWitJsonBody<TData>(string requestUri, TData value, CancellationToken cancellationToken)
        {
            var response = await client.PostAsync(requestUri, GetJsonContent(value), cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        
        private static StringContent GetJsonContent<TData>(TData value)
        {
            var content = JsonConvert.SerializeObject(value);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
        
        public void Dispose()
        {
            client.Dispose();
        }
    }
}