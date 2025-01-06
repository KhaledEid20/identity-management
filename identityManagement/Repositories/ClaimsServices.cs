using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace identityManagement.Repositories
{
    public class ClaimsServices :Base<User>, IClaims
    {
        private User _user;
        public ClaimsServices(UserManager<User> userManager , RoleManager<IdentityRole> roleManager) :base(userManager, roleManager)
        {
        }

        public async Task<string> addClaimToUser(string Email, string ClaimName , string ClaimValue)
        {
            _user = await ValidateUser(Email);
            if(_user == null){
                return "the user does not exist";
            }
            try{
                var claim = new Claim(ClaimName , ClaimValue);
                await _userManager.AddClaimAsync(_user , claim);
                return "The Claim Added Succesfully to the user";
            }
            catch{
                return "The Claim can't be added to the user";
            }
        }


        public async Task<string> addClaimToRole(string Rolename, string claimName, string ClaimValue)
        {
            var role = await ValidateRole(Rolename);
            if(role == null){
                return "The role does not exist";
            }
            try{
                var claim = new Claim(claimName , ClaimValue);
                await _roleManager.AddClaimAsync(role , claim);
                return "The Claim Added Succesfully to the role";
            }
            catch{
                return "The Claim can't be added to the role";
            }
        }
    }
}