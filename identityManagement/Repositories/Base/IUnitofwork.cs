using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.Repositories.Base
{
    public interface IUnitofwork
    {
        IAuthentication _authentication{get; set;}
        IClaims _claims{get; set;}
        IAdmin _admin{get; set;}
        IEmail _email{get; set;}
    }
}