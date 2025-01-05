using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace identityManagement.Repositories
{
    public class AuthenticationService : Base<User>,IAuthentication
    {
        private User _user;
        public readonly IConfiguration _configuration;
        public AuthenticationService(UserManager<User> userManager , RoleManager<IdentityRole> roleManager , IConfiguration configuration):base(userManager,roleManager)
        {
            this._configuration = configuration;
        }
        public async Task<authResult> Login(LoginDto user)
        {
            _user = await ValidateUser(user.Email);
            if(_user == null){
                return new authResult{
                  token = "",
                  RefreshToken="",
                  result = false,
                  error = "The User Does not exist"
                };
            }
            var Token = await GetToken();
            if(await _userManager.CheckPasswordAsync(_user,user.password)){
                return new authResult{
                    token = Token,
                    RefreshToken="",
                    result = true,
                    error = ""
                }; 
            }
            return new authResult{
                result = false,
                error = "username or password Invalid"
            };
        }
        public async Task<authResult> Register(RegisterDto register)
        {
            // create the user object
            _user = new User{
                UserName = register.UserName,
                Email = register.Email,
                PhoneNumber = register.phoneNumber
            };
            if(await ValidateUser(_user.Email)==null){
                //create the user with the password
                var result = await _userManager.CreateAsync(_user , register.password);
                var Token = await GetToken();
                if(result.Succeeded){
                    return new authResult{
                        token = Token,
                        RefreshToken = "Refresh Token Here",
                        result = true,
                        error = ""
                    };
                }
                return new authResult{
                    token = "",
                    RefreshToken = "",
                    result = false,
                    error = "The user can't be created"
                };
            }
            return new authResult{
                token = "",
                RefreshToken = "",
                result = false,
                error = "The User already Exist"
            };
        }

        #region Token Configuation
        public async Task<string> GetToken()
        {
            var sc = GetSigningCredentials();
            var claims = await GetClaims();
            var token = generateTokens(sc , claims);  // create the json web token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<List<Claim>> GetClaims(){
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name , _user.UserName));
            claims.Add(new Claim(ClaimTypes.Email , _user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var roles = await _userManager.GetRolesAsync(_user);
            foreach(var role in roles){
                claims.Add(new Claim(ClaimTypes.Role , role));
                var Role =await _roleManager.FindByNameAsync(role);
                if(Role != null){
                    var RoleClaims = await _roleManager.GetClaimsAsync(Role);
                    claims.AddRange(RoleClaims);
                }
            }
            return claims;
        }
        private SigningCredentials GetSigningCredentials(){
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")));
            return new SigningCredentials(secret , SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken generateTokens(SigningCredentials sc , List<Claim> claims){
            var jwtSetting = _configuration.GetSection("jwtSetting");
            var tokens = new JwtSecurityToken (
                issuer : jwtSetting["Issuer"],
                claims : claims,
                expires : DateTime.Now.AddMinutes(20),
                signingCredentials: sc
            );
            return tokens;
        }
        #endregion
    }
}