using CBH.Chat.Domain;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.Repository;
using CBH.Common.Security.Domain;
using CBH.Common.Security.Domain.DTO;

namespace CBH.Chat.Business
{
    public class ApiAuthenticationManager : IAuthenticationManager
    {
        private readonly IPartnerContext _partnerContext;

        public ApiAuthenticationManager(IPartnerContext partnerContext)
        {
            _partnerContext = partnerContext;
        }

        public AuthenticationResult Authenticate(Login login)
        {
            var authenticationResult = new ApiAuthenticationResult
            {
                LoginStatus = UserLoginStatus.NotSupportedLoginType
            };

            if (login.GetType() == typeof(ApiUserLogin))
            {
                authenticationResult.LoginStatus = UserLoginStatus.ValidUser;
            }

            return authenticationResult;
        }
    }
}
