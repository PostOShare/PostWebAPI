using System.Net.Http.Json;

namespace PostWebApiCommon.Helpers
{
    public class HttpClientHelper: IHttpClientHelper
    {
        private readonly HttpClient _httpClient;

        public HttpClientHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Rs> PostAsync<Rq, Rs>(string baseAddress, string uri, HttpContent content)
        {
            try
            {
                var response = await _httpClient.PostAsync(new Uri(new Uri(baseAddress), uri), content);
                return await response.Content.ReadFromJsonAsync<Rs>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}