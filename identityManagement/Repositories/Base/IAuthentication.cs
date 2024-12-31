using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.Repositories.Base
{
    public interface IAuthentication
    {
        Task<authResult> Register(RegisterDto register);
        Task<authResult> Login(LoginDto register);
        Task<string> GetToken();
    }
}