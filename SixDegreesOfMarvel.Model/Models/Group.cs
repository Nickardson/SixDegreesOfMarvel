using System.Collections.Generic;

namespace SixDegreesOfMarvel.Model.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string PrimaryImage { get; set; }
        public string FlavorText { get; set; }

        public List<Character> Characters { get; set; }
    }
}