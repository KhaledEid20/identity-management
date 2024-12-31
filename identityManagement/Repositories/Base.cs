using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;

namespace identityManagement.Repositories
{
    public class Base<T> : IBase<T> where T:class
    {
        private AppdbContext context;

        AppdbContext _context{get; set;}
        UserManager<User> _userManager{get; set;}
        RoleManager<Role> _roleManager{get; set;}
        public Base(AppdbContext context , UserManager<User> userManager , RoleManager<Role> roleManager)
        {
            this._context = context;
            this._roleManager = roleManager;
            this._userManager= userManager;
        }

        public Base(AppdbContext context)
        {
            this.context = context;
        }
        public async Task<authResult> generateTokens(User user)
        {
            if(user == null){
                return new authResult{
                    result = false,
                    error = "User do not exist"
                };
            }
            
            throw new NotImplementedException();
        }
    }
}