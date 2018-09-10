using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnBoarding.Models;

namespace OnBoarding.Models
{
    public class ResoluteContext : DbContext
    {
        public ResoluteContext(DbContextOptions<ResoluteContext > options)
            : base(options)
        {
        }

        public DbSet<OnBoarding.Models.Agent> Agent { get; set; }
        public DbSet<OnBoarding.Models.Organisation> organisation { get; set; }
        public DbSet<OnBoarding.Models.Department> Department { get; set; }
        public DbSet<OnBoarding.Models.EndUser> EndUser { get; set; }
        public DbSet<OnBoarding.Models.UserSocialId> UserSocialId { get; set; }
    }
}
