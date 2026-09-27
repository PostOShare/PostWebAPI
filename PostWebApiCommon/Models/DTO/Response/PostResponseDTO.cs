namespace PostWebApiCommon.Models.DTO.Response
{
    public class PostResponseDTO
    {
        public bool Result { get; set; }

        public string ErrorCode { get; set; } = string.Empty;

        public string ErrorDesc { get; set; } = string.Empty;

        public List<PostDto>? Posts { get; set; }
    }
}