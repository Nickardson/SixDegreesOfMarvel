using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SixDegreesOfMarvel.Model.Models;

namespace SixDegreesOfMarvel.Models
{
    public class CharacterGroupsResponse
    {
        public IEnumerable<GroupModel> Groups { get; set; }

        public CharacterGroupsResponse(IEnumerable<Group> groups)
        {
            Groups = groups
                .Select(group => new GroupModel(group));
        }
    }
}