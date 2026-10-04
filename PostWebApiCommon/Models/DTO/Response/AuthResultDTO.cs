using System.Text.Json.Serialization;

namespace PostWebApiCommon.Models.DTO.Response
{
    public class AuthResultDTO
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("result")]
        public bool Result { get; set; }
        
        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; } = string.Empty;

        [JsonPropertyName("errorDescription")]
        public string ErrorDescription { get; set; } = string.Empty;
    }
}