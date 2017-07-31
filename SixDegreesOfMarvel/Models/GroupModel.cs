using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SixDegreesOfMarvel.Model.Models;

namespace SixDegreesOfMarvel.Models
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Explored { get; set; }
        public string FlavorText { get; set; }
        public string PrimaryImage { get; set; }

        public GroupModel(Group group)
        {
            Id = group.GroupId;
            Name = group.Name;
            Explored = group.Explored;
            FlavorText = group.FlavorText;
            PrimaryImage = group.PrimaryImage;
        }
    }
}