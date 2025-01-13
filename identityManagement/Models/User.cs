using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.Models
{
    public class User : IdentityUser
    {
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate  { get; set; }
    }
}