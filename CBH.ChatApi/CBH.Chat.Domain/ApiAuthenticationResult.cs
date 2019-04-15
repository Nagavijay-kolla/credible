using CBH.Common.Security.Domain;
using CBH.Common.Security.Domain.DTO;

namespace CBH.Chat.Domain
{
    public class ApiAuthenticationResult : AuthenticationResult
    {
        public UserLoginStatus LoginStatus { get; set; }
    }
}
