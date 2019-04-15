using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CBH.Common.RedisCache;
using CBH.Common.Security.Domain;
using CBH.Common.Security.Interfaces.Business;
using CBH.ChatSignalR.Domain;
using CBH.ChatSignalR.Interfaces.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ClaimTypes = System.Security.Claims.ClaimTypes;

namespace CBH.ChatSignalR.Infrastructure
{
    public class AuthenticationMiddleware
    {
        private const string Bearer = "Bearer";
        private const string cwAuthTokenKey = "cwAuthToken";
        private readonly AuthenticationOptions _authenticationOptions;
        private readonly ICacheService _cacheService;
        private readonly RequestDelegate _next;
        private readonly ITokenManager _tokenManager;

        public AuthenticationMiddleware(RequestDelegate next, ITokenManager tokenManager,
            IOptions<AuthenticationOptions> options, ICacheService cacheService)
        {
            _next = next;
            _tokenManager = tokenManager;
            _authenticationOptions = options.Value;
            _cacheService = cacheService;
        }

        public Task Invoke(HttpContext context)
        {
            if (IsCurrentPathExcluded(context)) return _next(context);
            var token = GetTokenString(context);

            if (string.IsNullOrEmpty(token))
            {
                var currentPath = context.Request.Path.Value.ToLower();
                return SendUnauthorizedResponse($"Missing authentication token for url '{currentPath}'", context);
            }

            if (!TryGetUser(token, out string tokenValidationError, context, out var currentUser))
                return SendUnauthorizedResponse(tokenValidationError, context);

            if (!TryGetTransformedUser(out var claimRetrievalError, context, ref currentUser))
                return SendUnauthorizedResponse(claimRetrievalError, context);

            if (!CanAuthenticateUser(out var userAuthenticationError, context, currentUser))
                return SendUnauthorizedResponse(userAuthenticationError, context);

            return _next(context);
        }

        private bool IsCurrentPathExcluded(HttpContext context)
        {
            var currentPath = context.Request.Path.Value.ToLower();
            if (currentPath == "/" || currentPath == "") return true;

            if (currentPath.EndsWith(".css") || currentPath.EndsWith(".js") || currentPath.EndsWith(".json") || currentPath.EndsWith(".html"))
            {
                return true;
            }

            var isCurrentPathExcluded = _authenticationOptions.ExcludedRequestPaths.Any(path => currentPath.Equals(path, StringComparison.InvariantCultureIgnoreCase));
            return isCurrentPathExcluded;
        }

        private string GetTokenString(HttpContext currentContext)
        {
            var authHeader = currentContext.Request.Headers["Authorization"];

            if (!string.IsNullOrWhiteSpace(authHeader))
            {
                return authHeader.ToString().Replace(Bearer, "").Trim();
            }
            var cookieToken = currentContext.Request.Cookies[cwAuthTokenKey];

            if (!string.IsNullOrWhiteSpace(cookieToken))
            {
                return cookieToken;
            }

            var queryToken = currentContext.Request.Query[cwAuthTokenKey];

            if (!string.IsNullOrWhiteSpace(queryToken))
            {
                return queryToken;
            }

            return null;
        }

        private bool TryGetUser(string tokenString, out string error, HttpContext currentContext, out ApiUserLogin currentUser)
        {
            error = "";
            try
            {
                var principal = _tokenManager.ValidateToken(tokenString, _authenticationOptions.CurrentApplication);
                currentUser = new ApiUserLogin(principal);

                currentContext.Items["CurrentUser"] = currentUser;

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            currentUser = null;
            return false;
        }

        private bool TryGetTransformedUser(out string claimRetrievalError, HttpContext currentContext, ref ApiUserLogin currentUser)
        {
            var profileManager = currentContext.RequestServices.GetService<IProfileManager>();

            claimRetrievalError = "The encoded claims principal does not contain a profile code claim.";
            var profileCode = currentUser.GetClaimValue(ClaimTypes.Role);
            if (string.IsNullOrWhiteSpace(profileCode)) return false;
            claimRetrievalError = "The cache value for the security rights of the current profile code doesn't exist.";

            var rightsDictionary = _cacheService.Get($"ProfileRights_{currentUser.Domain}_{profileCode}_{ClaimCategories.Clients}",
                        60, () => profileManager.GetSecurityRights(profileCode, ClaimCategories.Clients));

            var claimsList = rightsDictionary.Select(r => new Claim(r, "")).ToList();
            if (claimsList == default(List<Claim>)) return false;

            var transformedPrincipal =
                profileManager.TransformClaimsPrincipal(currentUser.ClaimsPrincipal, claimsList);

            currentUser = new ApiUserLogin(transformedPrincipal);

            return true;
        }

        private bool CanAuthenticateUser(out string userValidationError, HttpContext currentContext, ApiUserLogin currentUser)
        {
            userValidationError = string.Empty;
            var authResult = currentContext.RequestServices.GetService<IAuthenticationManager>()
                .AuthenticateUser(currentUser);
            if (((ApiAuthenticationResult)authResult).LoginStatus != UserLoginStatus.ValidUser) return false;
            userValidationError = authResult.ErrorMessage;

            return true;
        }

        private Task<int> SendUnauthorizedResponse(string error, HttpContext currentContext)
        {
            currentContext.Response.OnStarting(() =>
            {
                currentContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                currentContext.Response.WriteAsync(FormatError(error));
                return Task.FromResult(0);
            });
            return Task.FromResult(0);
        }

        private string FormatError(string tokenValidationError)
        {
            return string.IsNullOrEmpty(tokenValidationError)
                ? ""
                : tokenValidationError.Replace(Environment.NewLine, " ").Replace("\n", " ");
        }
    }
}
