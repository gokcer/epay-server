using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epay3.Api.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace Epay3.Api.Models
{
    public partial class Epay3Context
    {
        private readonly AppTenant _appTenant;

        public Epay3Context(AppTenant appTenant)
        {
            _appTenant = appTenant;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_appTenant.ConnectionString);
            }
        }
    }
}
