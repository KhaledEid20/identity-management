using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.DTOs
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public TokenDto(string AccessToken ,string RefreshToken)
        {
            this.AccessToken = AccessToken;
            this.RefreshToken = RefreshToken;
        }
    }
}