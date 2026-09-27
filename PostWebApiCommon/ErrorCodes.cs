namespace PostWebApiCommon
{
    public class ErrorCodes
    {       
        public const string Success = "00000";
        public const string PostNotFound = "00001";
        public const string UserNotAuthorizedToCreatePost = "00002";
        public const string UserNotAuthorizedToDeletePost = "00003";
        public const string FailedToDeletePost = "00004";
        public const string FailedToUpdatePost = "00005";
    }
}
