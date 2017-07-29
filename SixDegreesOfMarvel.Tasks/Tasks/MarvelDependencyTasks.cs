using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixDegreesOfMarvel.Model.Models;
using SixDegreesOfMarvel.Repository;

namespace SixDegreesOfMarvel.Tasks.Tasks
{
    public class MarvelDependencyTasks
    {
        public List<Character> DoThing()
        {
            var x = new MarvelDbContext();
            x.Characters.Add(new Character()
            {
                Name = "Billy Bob",
                FlavorText = "He's a GOAT"
            });

            x.SaveChanges();

            return x.Characters.ToList();
        }

        //public List<Page> PagesLinkedTo(Page page)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Page> PagesThatLinkTo(Page page)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Category> GetCategoriesOnPage(Page page)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Category> GetPagesInCategory(Page page)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
