using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.Repositories
{
    public class ClaimsServices : IClaims
    {
        UserManager<User> _userManager{get; set;}
        RoleManager<Role> _roleManager{get; set;}
        public ClaimsServices(UserManager<User> userManager , RoleManager<Role> roleManager)
        {
            this._roleManager = roleManager;
            this._userManager= userManager;
        }
    }
}