using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using identityManagement.Repositories.Base;
using Microsoft.IdentityModel.Tokens;

namespace identityManagement.Repositories
{
    public class AuthenticationService : Base<User>,IAuthentication
    {
        public User _user;
        public UserManager<User> _userManager{get; set;}
        public RoleManager<Role> _roleManager{get; set;}
        private readonly IConfiguration _configuration;
        public AuthenticationService(AppdbContext context, UserManager<User> userManager , RoleManager<Role> roleManager , IConfiguration configuration) : base(context)
        {
            this._roleManager = roleManager;
            this._userManager= userManager;
            this._configuration = configuration;
        }
        public async Task<authResult> Login(LoginDto user)
        {
            _user = await _userManager.FindByEmailAsync(user.Email);
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
            var user = new User{
                UserName = register.UserName,
                Email = register.Email,
                PhoneNumber = register.phoneNumber
            };
            //create the user with the password
            var result = await _userManager.CreateAsync(user , register.password);
            _user = user;
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
                token = Token,
                RefreshToken = "",
                result = false,
                error = "The user can't be created"
            };
        }
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
                    claims.Add(new Claim(ClaimTypes.Role , role));
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
        
    }
}