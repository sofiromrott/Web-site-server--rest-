using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationREST.Data
{
    public class WebApplicationRESTContext : DbContext
    {
        internal object ReadStudent;

        public WebApplicationRESTContext(DbContextOptions<WebApplicationRESTContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = default!;

        //public DbSet<ReadStudent> ReadStudent { get; set; }
    }
}
