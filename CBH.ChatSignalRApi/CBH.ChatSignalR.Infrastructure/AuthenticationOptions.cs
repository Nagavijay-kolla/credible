using System.Collections.Generic;

namespace CBH.ChatSignalR.Infrastructure
{
    public class AuthenticationOptions
    {
        public List<string> ExcludedRequestPaths { get; set; }
        public string CurrentApplication { get; set; }
    }
}
