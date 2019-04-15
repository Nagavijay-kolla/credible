using CBH.Common.Security.Domain;
using CBH.Common.Security.Domain.DTO;

namespace CBH.ChatSignalR.Domain
{
    public class ApiAuthenticationResult : AuthenticationResult
    {
        public ApiAuthenticationResult()
        {

        }

        public UserLoginStatus LoginStatus { get; set; }
    }
}
