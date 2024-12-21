using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace identityManagement.Data
{
    public class AppdbContext : IdentityDbContext<IdentityUser>
    {
        public AppdbContext(DbContextOptions<AppdbContext> options):base(options)
        {
            
        }
    }
}