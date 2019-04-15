using CBH.Common.Security.Domain;
using CBH.Common.Security.Domain.DTO;
using CBH.ChatSignalR.Domain;
using CBH.ChatSignalR.Interfaces.Business;
using CBH.ChatSignalR.Interfaces.Repository;

namespace CBH.ChatSignalR.Business
{
    public class ApiAuthenticationManager : IAuthenticationManager
    {
        private readonly IPartnerContext _partnerContext;

        public ApiAuthenticationManager(IPartnerContext partnerContext)
        {
            _partnerContext = partnerContext;
        }

        public AuthenticationResult AuthenticateUser(Login login)
        {
            var authenticationResult = AuthenticateSpecificUser(login);
            return authenticationResult;
        }

        public AuthenticationResult AuthenticateSpecificUser(Login login)
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
