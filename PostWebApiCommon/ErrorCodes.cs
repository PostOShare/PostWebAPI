namespace PostWebApiCommon
{
    public class ErrorCodes
    {       
        public const string Success = "0000";
        public const string PostNotFound = "0001";
        public const string UserNotAuthorizedToCreatePost = "0002";
        public const string UserNotAuthorizedToDeletePost = "0003";
        public const string UserNotAuthorizedToUpdatePost = "0004";
        public const string UserNotAuthorizedToGetPosts = "0005";
        public const string UsernameTokenError = "0006";
        public const string FailedToDeletePost = "0007";        
        public const string InternalServerError = "0008";
        public const string FailedToUpdatePost = "0009";
    }
}
