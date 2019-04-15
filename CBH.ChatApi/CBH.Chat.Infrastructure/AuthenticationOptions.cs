using System.Collections.Generic;

namespace CBH.Chat.Infrastructure
{
    public class AuthenticationOptions
    {
        public List<string> ExcludedRequestPaths { get; set; }
        public string CurrentApplication { get; set; }
    }
}
