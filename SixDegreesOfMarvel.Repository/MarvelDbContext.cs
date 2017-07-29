using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixDegreesOfMarvel.Model.Models;

namespace SixDegreesOfMarvel.Repository
{
    public class MarvelDbContext : DbContext
    {
        public MarvelDbContext() : base("SixDegreesOfMarvel")
        {
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
