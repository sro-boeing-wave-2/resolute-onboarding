using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnBoarding.Models;

namespace OnBoarding.Models
{
    public class OnBoardingContext : DbContext
    {
        public OnBoardingContext (DbContextOptions<OnBoardingContext> options)
            : base(options)
        {
        }

        public DbSet<OnBoarding.Models.Agent> Agent { get; set; }
        public DbSet<OnBoarding.Models.Organisation> Organisation { get; set; }
        public DbSet<OnBoarding.Models.Department> Department { get; set; }
        public DbSet<OnBoarding.Models.EndUser> EndUser { get; set; }
        public DbSet<OnBoarding.Models.UserSocialId> UserSocialId { get; set; }

    }
}
