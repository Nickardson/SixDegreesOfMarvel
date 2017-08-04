using SixDegreesOfMarvel.Model.Models;

namespace SixDegreesOfMarvel.Models
{
    /// <summary>
    /// Represents a character, and how the chain got to that character.
    /// </summary>
    public class ConnectionModel
    {
        /// <summary>
        /// The character which is part of the chain.
        /// </summary>
        public CharacterModel Character { get; set; }
        /// <summary>
        /// The group this connection came from. The starting character has a null 'KnownFromGroup', unless the graph is intended to be circular.
        /// </summary>
        public GroupModel KnownFromGroup { get; set; }

        public ConnectionModel(Character character, Group group)
        {
            Character = character != null ? new CharacterModel(character) : null;
            KnownFromGroup = group != null ? new GroupModel(group) : null;
        }
    }
}