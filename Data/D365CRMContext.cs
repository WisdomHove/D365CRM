using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using D365CRM.Model;

namespace D365CRM.Data
{
    public class D365CRMContext : DbContext
    {
        public D365CRMContext (DbContextOptions<D365CRMContext> options)
            : base(options)
        {
        }

        public DbSet<D365CRM.Model.Product> Product { get; set; }
    }
}
