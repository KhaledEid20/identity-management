using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace identityManagement.Data
{
    public class AppdbContext : IdentityDbContext<User , Role , string>
    {
        public AppdbContext(DbContextOptions<AppdbContext> options):base(options)
        {
            
        }
    }
}