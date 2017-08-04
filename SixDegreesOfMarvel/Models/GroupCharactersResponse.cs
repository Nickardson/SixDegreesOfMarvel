using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SixDegreesOfMarvel.Model.Models;

namespace SixDegreesOfMarvel.Models
{
    public class GroupCharactersResponse
    {
        public IEnumerable<CharacterModel> Characters { get; set; }

        public GroupCharactersResponse(IEnumerable<Character> characters)
        {
            Characters = characters
                .Select(character => new CharacterModel(character));
        }
    }
}