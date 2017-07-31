using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixDegreesOfMarvel.Model.Models;

namespace SixDegreesOfMarvel.Repository
{
    public class MarvelDbInitializer : DropCreateDatabaseIfModelChanges<MarvelDbContext>
    {
        protected override void Seed(MarvelDbContext context)
        {
            //context.Characters.Add(new Character()
            //{
            //    Name = ""
            //});

            context.SaveChanges();
        }
    }
}
