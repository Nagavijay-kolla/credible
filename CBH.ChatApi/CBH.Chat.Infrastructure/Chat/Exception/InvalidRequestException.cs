
namespace CBH.Chat.Infrastructure.Chat.Exception
{
    public class InvalidRequestException : System.Exception
    {
        public InvalidRequestException()
        {

        }

        public InvalidRequestException(string message) : base(message)
        {
        }
    }
}
