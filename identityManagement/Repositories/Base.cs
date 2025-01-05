using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.IdentityModel.Tokens;

namespace identityManagement.Repositories
{
    public class Base<T> : IBase<T> where T:class
    {
        public UserManager<User> _userManager;
        public RoleManager<IdentityRole> _roleManager;


        public Base(UserManager<User> userManager , RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
            this._userManager= userManager;
        }

        public async Task<User> ValidateUser(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if(user == null){
                return null;
            }
            return user;
        }

        public async Task<IdentityRole> ValidateRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if(role == null){
                return null;
            }
            return role;
        }
    }
}