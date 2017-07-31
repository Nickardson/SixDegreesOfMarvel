using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SixDegreesOfMarvel.Model.Models;

namespace SixDegreesOfMarvel.Models
{
    public class CharacterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Explored { get; set; }
        public string FlavorText { get; set; }
        public string PrimaryImage { get; set; }
        
        public CharacterModel(Character character)
        {
            Id = character.CharacterId;
            Name = character.Name;
            Explored = character.Explored;
            FlavorText = character.FlavorText;
            PrimaryImage = character.PrimaryImage;
        }
    }
}