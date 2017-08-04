using System.Data.Entity;
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
        public DbSet<CharacterGroup> CharacterGroups { get; set; }
    }
}
