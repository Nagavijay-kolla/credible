using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CBH.Common.Security.Domain;
using CBH.Common.Security.Domain.DTO;
using ClaimTypes = CBH.Common.Security.Domain.ClaimTypes;

namespace CBH.ChatSignalR.Domain
{
    public class ApiUserLogin : SessionUserLogin
    {
        private readonly ClaimsPrincipal _claimsPrincipal;

        public ApiUserLogin(ClaimsPrincipal principal)
        {
            _claimsPrincipal = principal;
            Domain = GetPartnerDomain(principal.Identity.Name);
            UserName = GetUsername(principal.Identity.Name);
            ApplicationId = Applications.IntegratedCare;
        }

        public int EmployeeId => GetEmployeeId();
        public List<Claim> Claims => _claimsPrincipal.Claims.ToList();
        public ClaimsPrincipal ClaimsPrincipal => _claimsPrincipal;

        public bool HasSecurityRight(string securityRight)
        {
            return Claims.FirstOrDefault(c => c.Type == securityRight) != null;
        }
        public string GetClaimValue(string claimType)
        {
            return Claims.FirstOrDefault(c => claimType == c.Type)?.Value;
        }

        private string GetPartnerDomain(string identityName)
        {
            return identityName.Substring(0, identityName.IndexOf("/", StringComparison.Ordinal));
        }
        private string GetUsername(string identityName)
        {
            return identityName.Substring(identityName.IndexOf("/", StringComparison.Ordinal) + 1);
        }
        private int GetEmployeeId()
        {
            var claimValue = Claims.SingleOrDefault(c => c.Type == ClaimTypes.EmployeeId)?.Value;
            int.TryParse(claimValue, out var employeeId);
            return employeeId;
        }
    }
}
