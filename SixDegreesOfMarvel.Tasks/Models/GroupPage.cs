using System.Collections.Generic;

namespace SixDegreesOfMarvel.Tasks.Models
{
    public class GroupPage : Page
    {
        public List<CharacterPage> Characters { get; set; }
    }
}