using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.Repositories
{
    public class AdminServices : Base<User> , IAdmin
    {
        private User _user;
        public AdminServices(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
        }

        public async Task<string> assignRoleToUser(string Email, string Role)
        {
            _user = await ValidateUser(Email);
            if (_user != null){
                var role = await ValidateRole(Role);
                if(role != null){
                    await _userManager.AddToRoleAsync(_user , Role);
                    return $"The Role {Role} added to the user {_user.UserName}";
                }
                else{
                    return "The Role doesn not Exist";
                }
            }
            else{
                return "The User doesn't exist";
            }
        }

        public async Task<string> AddRole(string Role)
        {
            var role = await ValidateRole(Role);
            if(role == null){
                await _roleManager.CreateAsync(new IdentityRole(Role));
                return $"The {Role} have been added to the Roles table";
            }
            else{
                return "The role already exist";
            }
        }
    }
}