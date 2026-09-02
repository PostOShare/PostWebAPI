using PostWebApiCommon.Models.DTO.Request;
using PostWebApiCommon.Models.DTO.Response;

namespace PostWebApiService.Services
{
    public interface IPostService
    {
        Task<BaseResponseDTO> CreatePost(PostWebApiCommon.Models.DTO.Request.PostDto postDto);
    }
}