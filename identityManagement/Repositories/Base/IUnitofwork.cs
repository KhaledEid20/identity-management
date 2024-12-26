using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.Repositories.Base
{
    public interface IUnitofwork
    {
        IAccount _account{get; set;}
        IClaims _claims{get; set;}
        IAdmin _admin{get; set;}
    }
}