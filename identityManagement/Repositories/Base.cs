using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.Repositories
{
    public class Base<T> : IBase<T> where T:class
    {
        AppdbContext _context{get; set;}
        UserManager<User> _userManager{get; set;}
        RoleManager<Role> _roleManager{get; set;}
        public Base(AppdbContext context , UserManager<User> userManager , RoleManager<Role> roleManager)
        {
            this._context = context;
            this._roleManager = roleManager;
            this._userManager= userManager;
        }
    }
}