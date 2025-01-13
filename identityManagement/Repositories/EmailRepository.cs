using System;

namespace identityManagement.Repositories;

public class EmailRepository : Base<User>, IEmail
{
    public EmailRepository(UserManager<User> userManager , RoleManager<IdentityRole> roleManager) : base(userManager , roleManager)
    {
    }

    public async Task<string> sendEmail(string email, string code)
    {
        if(email == null || code == null){
            return "1";
        }
        var user = await ValidateUser(email);
        if(user == null){
            return "2";
        }
        var verified = await _userManager.ConfirmEmailAsync(user, code);
        if(verified.Succeeded){
            return "3";
        }
        return "4";
    }
}
