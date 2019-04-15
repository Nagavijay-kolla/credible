using Microsoft.AspNetCore.Builder;

namespace CBH.ChatSignalR.Infrastructure
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
