using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FortniteCompetition.Models
{
    public class CompetitionDataContext : DbContext
    {
        public DbSet<CompetitionPost> CompetitionPost { get; set; }

        public CompetitionDataContext(DbContextOptions<CompetitionDataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
