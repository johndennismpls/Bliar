using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bliar.Data.Models
{
    public class BliarContext : DbContext
    {
        public BliarContext(DbContextOptions<BliarContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}
