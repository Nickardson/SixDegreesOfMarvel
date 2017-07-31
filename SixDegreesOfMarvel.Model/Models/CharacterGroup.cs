using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
