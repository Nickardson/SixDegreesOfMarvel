using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SixDegreesOfMarvel.Model.Models;

namespace SixDegreesOfMarvel.Models
{
    public class GroupCharactersResponse
    {
        public IEnumerable<string> Characters { get; set; }

        public GroupCharactersResponse(IEnumerable<Character> characters)
        {
            Characters = characters
                .Select(character => character.Name);
        }
    }
}