using System.Net.Http.Json;

namespace PostWebApiCommon.Helpers
{
    public class ExternalHttpClientHelper: IExternalHttpClientHelper
    {
        public readonly IHttpClientFactory _httpClientFactory;

        public ExternalHttpClientHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TResponse> PostAsyncExternal<TRequest, TResponse>(string baseAddress, string uri, TRequest content)
        {
            var client = _httpClientFactory.CreateClient(Constants.ExternalHttpClient);
            client.BaseAddress = new Uri(baseAddress);
            var response = await client.PostAsync(uri, JsonContent.Create(content));
            var responseContent = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<TResponse>(responseContent)!;
        }
    }
}