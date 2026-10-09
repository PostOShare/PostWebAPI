namespace PostWebApiCommon.Helpers
{
    public interface IExternalHttpClientHelper
    {
        Task<TResponse> PostAsyncExternal<TRequest, TResponse>(string baseAddress, string uri, TRequest content);
    }
}