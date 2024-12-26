using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using identityManagement.Repositories.Base;

namespace identityManagement.Repositories
{
    public class Unitofwork : IUnitofwork
    {
        public IAccount _account { get; set; }
        public IClaims _claims { get; set; }
        public IAdmin _admin { get; set; }
        public Unitofwork(IAccount account , IClaims claims , IAdmin admin)
        {
            this._account = account;
            this._claims = claims;
            this._admin = admin;
        }
    }
}