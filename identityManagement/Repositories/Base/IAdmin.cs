using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.Repositories.Base
{
    public interface IAdmin
    {
        Task<string> assignRoleToUser(string Email , string Role);
        Task<string> AddRole(string Role);
    }
}