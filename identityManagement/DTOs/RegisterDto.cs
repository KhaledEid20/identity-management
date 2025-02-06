using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.DTOs
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public string phoneNumber { get; set; }
    }
}