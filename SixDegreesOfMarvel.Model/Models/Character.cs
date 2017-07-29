using System.Collections.Generic;

namespace SixDegreesOfMarvel.Model.Models
{
    public class Character
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public string PrimaryImage { get; set; }
        public string FlavorText { get; set; }

        public List<Group> Groups { get; set; }
    }
}
