namespace PostWebApiCommon.Helpers
{
    public interface IHttpClientHelper
    {
        Task<Rs> PostAsync<Rq, Rs>(string baseAddress, string uri, HttpContent content);
    }
}