using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PostWebApiCommon.Models.DTO.Request
{
    public class ValidateTokenRequestDTO
    {
        [Required]
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}