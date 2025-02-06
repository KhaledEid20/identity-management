using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
            var EmailConfirmed = await _userManager.IsEmailConfirmedAsync(_user);
            if(!EmailConfirmed){
                return new authResult{
                    error = "The Email is not confirmed",
                };
            }
            var Token = await GetToken(Exp:true);
            if(await _userManager.CheckPasswordAsync(_user,user.password)){
                return new authResult{
                    token = Token.AccessToken,
                    RefreshToken=Token.RefreshToken,
                    result = true,
                    error = ""
                }; 
            }
            return new authResult{
                result = false,
                error = "username or password Invalid"
            };
        }
        public async Task<resultRegisterDTO> Register(RegisterDto register)
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

                if(result.Succeeded){
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(_user);
                    return new resultRegisterDTO{
                        email = _user.Email,
                        code = code
                    };
                } 
                return new resultRegisterDTO{
                    error = "The user can't be created"
                };
            };
            return new resultRegisterDTO{
                error = $"{_user.Email} The user alreay exist"
            };
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = getPrincipalFromExpiryDate(tokenDto.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            var exp = user.ExpirationDate;
            var dn = DateTime.UtcNow;
            if(user == null || user.RefreshToken != tokenDto.RefreshToken || user.ExpirationDate <= DateTime.UtcNow){
                throw new Exception("Invalid Client Exception");
            }
            _user = user;
            // Console.WriteLine(user.RefreshToken , tokenDto.RefreshToken);
            // Console.WriteLine($"{user.ExpirationDate}" , DateTime.UtcNow);
            return await GetToken(Exp:false);
        }
        public async Task<TokenDto> GetToken(bool Exp)
        {
            var sc = GetSigningCredentials();
            var claims = await GetClaims();
            var token = generateTokens(sc , claims);  // create the json web token
            // return 
            var refreshtoken = generateRefreshToken();
            _user.RefreshToken = refreshtoken;
            if(Exp){
                _user.ExpirationDate = DateTime.UtcNow.AddMinutes(7);
            }
            await _userManager.UpdateAsync(_user);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new TokenDto(accessToken, refreshtoken);
        }

        #region Token Configuation

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
            var jwtSetting = _configuration.GetSection("jwtSettings");
            var tokens = new JwtSecurityToken (
                issuer : jwtSetting["Issuer"],
                claims : claims,
                expires : DateTime.UtcNow.AddSeconds(5),
                signingCredentials: sc
            );
            return tokens;
        }

        private string generateRefreshToken(){
            var randomNumber = new Byte[32];
            using(var rt = RandomNumberGenerator.Create()){
                rt.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        // Take the principal of the Expired Token
        private ClaimsPrincipal getPrincipalFromExpiryDate(string token){
            var jwtSetting = _configuration.GetSection("jwtSettings");
            var validateToken = new TokenValidationParameters{ // Validate the token
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))
                ),
                ValidIssuer = jwtSetting["Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token , validateToken , out securityToken); // give the token TokenValidation and the output goes to the output
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if(jwtSecurityToken == null || !jwtSecurityToken.Header.Alg
            .Equals(SecurityAlgorithms.HmacSha256 , StringComparison.InvariantCultureIgnoreCase)){

                throw new SecurityTokenException("Invalid Token");

            }
            return principal;
        }
        #endregion
    }
}
