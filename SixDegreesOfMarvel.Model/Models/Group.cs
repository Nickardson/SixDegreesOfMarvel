using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SixDegreesOfMarvel.Model.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        [Required]
        public string Name { get; set; }
        public string PrimaryImage { get; set; }
        public string FlavorText { get; set; }

        public bool Explored { get; set; }

        public virtual ICollection<CharacterGroup> CharacterGroups { get; set; } = new List<CharacterGroup>();
    }
}