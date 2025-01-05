using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.Repositories.Base
{
    public interface IClaims
    {
        Task<string> addClaimToUser(string Email , string ClaimName , string ClaimValue);
        Task<string> addClaimToRole(string Rolename , string claimName, string ClaimValue);

    }
}