using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SixDegreesOfMarvel.Model.Models;

namespace SixDegreesOfMarvel.Models
{
    public class ConnectionModel
    {
        public CharacterModel Character { get; set; }
        public GroupModel KnownFromGroup { get; set; }

        public ConnectionModel(Character character, Group group)
        {
            Character = character != null ? new CharacterModel(character) : null;
            KnownFromGroup = group != null ? new GroupModel(group) : null;
        }
    }
}