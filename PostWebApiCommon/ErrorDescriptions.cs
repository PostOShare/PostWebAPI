namespace PostWebApiCommon
{
    public class ErrorDescriptions
    {
        public const string Success = "Success";
        public const string PostNotFound = "Post not found";
        public const string UserNotAuthorizedToCreatePost = "User is not authorized to create this post";
        public const string UserNotAuthorizedToDeletePost = "User is not authorized to delete this post";
        public const string UserNotAuthorizedToUpdatePost = "User is not authorized to update this post";
        public const string UserNotAuthorizedToGetPosts = "User is not authorized to get posts";
        public const string FailedToDeletePost = "Failed to delete post";
        public const string FailedToUpdatePost = "Failed to update post";
        public const string UsernameTokenError = "Invalid username and/or expired token";
    }
}