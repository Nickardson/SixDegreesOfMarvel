using System.Data.Entity;
using SixDegreesOfMarvel.Model.Models;

namespace SixDegreesOfMarvel.Repository
{
    public class MarvelDbInitializer : DropCreateDatabaseIfModelChanges<MarvelDbContext>
    {
        protected override void Seed(MarvelDbContext context)
        {
            CreateMarvelCharacters(context);
        }

        public void CreateMarvelCharacters(MarvelDbContext context)
        {
            // The SHIELD page on the marvel wiki has problems sometimes.
            // So this doesn't reflect poorly on the API,
            // We pre-add some SHIELD members so it works out of the gate.
            var group = AddGroupAsExplored(context, "S.H.I.E.L.D.");
            AddCharacterInGroup(context, "Iron Man (Anthony Stark)", group);
            AddCharacterInGroup(context, "Hill, Maria", group);
            AddCharacterInGroup(context, "Spider-Woman (Jessica Drew)", group);
            AddCharacterInGroup(context, "Dugan, Dum-Dum", group);
            AddCharacterInGroup(context, "Gorilla Man (Kenneth Hale)", group);
            AddCharacterInGroup(context, "Johnson, Daisy", group);
            AddCharacterInGroup(context, "Black Widow (Natasha Romanova)", group);

            context.SaveChanges();
        }

        private void AddCharacterInGroup(MarvelDbContext context, string name, Group group)
        {
            var character = context.Characters.Add(new Character()
            {
                Name = name
            });

            context.CharacterGroups.Add(new CharacterGroup()
            {
                Group = group,
                Character = character
            });
        }

        private Group AddGroupAsExplored(MarvelDbContext context, string group)
        {
            return context.Groups.Add(new Group()
            {
                Name = group,
                Explored = true
            });
        }
    }
}
