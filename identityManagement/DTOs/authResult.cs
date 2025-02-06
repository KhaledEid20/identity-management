using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityManagement.DTOs
{
    public class authResult
    {
        public string token { get; set; }
        public string RefreshToken { get; set; }
        public bool result { get; set; } = false;
        public string error { get; set; }
    }
}