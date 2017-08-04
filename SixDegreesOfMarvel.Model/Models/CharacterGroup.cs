using System.ComponentModel.DataAnnotations;

namespace SixDegreesOfMarvel.Model.Models
{
    public class CharacterGroup
    {
        [Key]
        public int Id { get; set; }
        public virtual Character Character { get; set; }
        public virtual Group Group { get; set; }
    }
}
