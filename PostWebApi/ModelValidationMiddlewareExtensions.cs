using PostWebApiCommon.Middlewares;

namespace PostWebApi
{
    public static class ModelValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseModelValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseMiddleware>();
        }
    }
}