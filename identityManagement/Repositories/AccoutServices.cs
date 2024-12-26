using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using identityManagement.Repositories.Base;

namespace identityManagement.Repositories
{
    public class AccoutServices : IAccount
    {
        UserManager<User> _userManager{get; set;}
        RoleManager<Role> _roleManager{get; set;}
        public AccoutServices(UserManager<User> userManager , RoleManager<Role> roleManager)
        {
            this._roleManager = roleManager;
            this._userManager= userManager;
        }
    }
}