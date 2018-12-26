using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MMS.Models.SetupModels;

namespace MMS.Models
{
    public class MMSContext : DbContext
    {
        public MMSContext() : base("name=DefaultConnection")
        {
            this.Configuration.ProxyCreationEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Bazaar> Bazaars { get; set; }
        public DbSet<TotalMealPerDay> TotalMealPerDays { get; set; }
    }
}