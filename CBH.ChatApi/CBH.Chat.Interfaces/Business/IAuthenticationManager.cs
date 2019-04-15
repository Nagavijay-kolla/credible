using CBH.Common.Security.Domain.DTO;

namespace CBH.Chat.Interfaces.Business
{
    public interface IAuthenticationManager
    {
        AuthenticationResult Authenticate(Login login);
    }
}