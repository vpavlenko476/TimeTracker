using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using JsonSerializer = System.Text.Json.JsonSerializer;

//todo заменить newtonsoft
namespace Jira.Client
{
    public abstract class BaseClient : IDisposable
    {
        private readonly HttpClient client; 
        protected BaseClient(Uri baseUrl, string token)
        {
            client = new HttpClient
            {
                BaseAddress = baseUrl,
            };
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        protected async Task<TResponse> Get<TResponse>(string url, CancellationToken cancellationToken)
        {
            var response = await client.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            var responseContents = await response.Content.ReadAsStreamAsync(cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            JsonNode jNode = JsonNode.Parse(responseContent);
            var x = JsonSerializer.Deserialize<TResponse>(responseContents);
            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }
        
        public async Task<TResponse> PostWitJsonBody<TResponse, TData>(string requestUri, TData value, CancellationToken cancellationToken)
        {
            var response = await client.PostAsync(requestUri, GetJsonContent(value), cancellationToken);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync(cancellationToken);

            return (TResponse) JsonConvert.DeserializeObject(stringContent, typeof(TResponse));
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