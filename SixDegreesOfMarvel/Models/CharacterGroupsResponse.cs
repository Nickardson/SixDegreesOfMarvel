using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SixDegreesOfMarvel.Model.Models;

namespace SixDegreesOfMarvel.Models
{
    public class CharacterGroupsResponse
    {
        public IEnumerable<string> Groups { get; set; }

        public CharacterGroupsResponse(IEnumerable<Group> groups)
        {
            Groups = groups
                .Select(group => group.Name);
        }
    }
}