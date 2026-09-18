namespace PostWebApiCommon.Models.DTO.Response
{
    public class PostDto
    {
        public bool Result { get; set; }

        public string ErrorCode { get; set; } = string.Empty;

        public string ErrorDesc { get; set; } = string.Empty;
    }
}