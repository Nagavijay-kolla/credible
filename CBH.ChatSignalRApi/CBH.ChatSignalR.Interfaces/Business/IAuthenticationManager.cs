using CBH.Common.Security.Domain.DTO;

namespace CBH.ChatSignalR.Interfaces.Business
{
    public interface IAuthenticationManager
    {
        AuthenticationResult AuthenticateUser(Login login);
    }
}
