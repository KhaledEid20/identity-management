using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.Repositories.Base
{
    public interface IBase<T> where T : class
    {
        Task<authResult> generateTokens(User user);
    }
}