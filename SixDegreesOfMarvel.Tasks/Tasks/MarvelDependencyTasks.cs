using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarvelApi.Tasks;
using SixDegreesOfMarvel.Model.Models;
using SixDegreesOfMarvel.Repository;
using SixDegreesOfMarvel.Tasks.DTO;

namespace SixDegreesOfMarvel.Tasks.Tasks
{
    public class MarvelDependencyTasks
    {
        private readonly MarvelDbContext _marvelDbContext = new MarvelDbContext();
        private MarvelApiTasks _marvelApiTasks;

        public MarvelDependencyTasks(MarvelApiTasks marvelApiTasks)
        {
            _marvelApiTasks = marvelApiTasks;
        }

        public async Task<bool> ClearCache()
        {
            _marvelDbContext.CharacterGroups.RemoveRange(_marvelDbContext.CharacterGroups);
            _marvelDbContext.Groups.RemoveRange(_marvelDbContext.Groups);
            _marvelDbContext.Characters.RemoveRange(_marvelDbContext.Characters);

            await _marvelDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Character>> GetAllCharacters()
        {
            return await _marvelDbContext.Characters.ToListAsync();
        }

        public async Task<List<Group>> GetAllGroups()
        {
            return await _marvelDbContext.Groups.ToListAsync();
        }

        public async Task<Character> GetCharacter(int id)
        {
            return await _marvelDbContext.Characters.FindAsync(id);
        }

        public async Task<Character> GetCharacterByName(string characterName)
        {
            characterName = _marvelApiTasks.NormalizePageName(characterName);

            // TODO: test toupper
            return await _marvelDbContext.Characters.SingleOrDefaultAsync(
                character => character.Name == characterName);
        }

        public async Task<Group> GetGroup(int id)
        {
            return await _marvelDbContext.Groups.FindAsync(id);
        }

        public async Task<Group> GetGroupByName(string groupName)
        {
            groupName = _marvelApiTasks.NormalizePageName(groupName);

            // TODO: test toupper
            return await _marvelDbContext.Groups.SingleOrDefaultAsync(
                group => group.Name == groupName);
        }

        public async Task<List<Character>> GetCharacterAffiliations(string groupName)
        {
            groupName = _marvelApiTasks.NormalizePageName(groupName);

            var group = await GetGroupByName(groupName);

            // create a skeleton unexplorerd character if we don't know about this character currently.
            if (group == null)
            {
                group = new Group()
                {
                    Name = groupName
                };
                _marvelDbContext.Groups.Add(group);
            }

            // if we haven't explored this character...
            if (!group.Explored)
            {
                // determine which groups it is related to.
                var affiliations = await _marvelApiTasks.GetCharacterAffiliations(group.Name);

                _marvelDbContext.CharacterGroups.RemoveRange(group.CharacterGroups);
                //group.CharacterGroups.Clear();

                foreach (var affilation in affiliations)
                {
                    // then link them up, creating a skeleton group if needed.
                    var character = await GetCharacterByName(affilation);
                    if (character == null)
                    {
                        character = new Character()
                        {
                            Name = affilation
                        };
                        _marvelDbContext.Characters.Add(character);
                    }

                    var characterGroup = new CharacterGroup()
                    {
                        Character = character,
                        Group = group
                    };

                    // TODO: test intended behavior
                    _marvelDbContext.CharacterGroups.Add(characterGroup);
                }

                group.Explored = true;
            }

            await _marvelDbContext.SaveChangesAsync();
            
            return group.CharacterGroups
                .Select(x => x.Character)
                .ToList();
        }

        public async Task<List<Group>> GetGroupAffiliations(string characterName)
        {
            characterName = _marvelApiTasks.NormalizePageName(characterName);

            var character = await GetCharacterByName(characterName);

            // create a skeleton unexplorerd character if we don't know about this character currently.
            if (character == null)
            {
                character = new Character()
                {
                    Name = characterName
                };
                _marvelDbContext.Characters.Add(character);
            }
            
            // if we haven't explored this character...
            if (!character.Explored)
            {
                // determine which groups it is related to.
                var affiliations = await _marvelApiTasks.GetGroupAffiliations(character.Name);
                _marvelDbContext.CharacterGroups.RemoveRange(character.CharacterGroups);
                //character.CharacterGroups.Clear();

                foreach (var affilation in affiliations)
                {
                    // then link them up, creating a skeleton group if needed.
                    var group = await GetGroupByName(affilation);
                    if (group == null)
                    {
                        group = new Group()
                        {
                            Name = affilation
                        };
                        _marvelDbContext.Groups.Add(group);
                    }
                    
                    var characterGroup = new CharacterGroup()
                    {
                        Character = character,
                        Group = group
                    };

                    // TODO: test intended behavior
                    _marvelDbContext.CharacterGroups.Add(characterGroup);
                }

                character.Explored = true;
            }
            
            await _marvelDbContext.SaveChangesAsync();

            return character.CharacterGroups
                .Select(x => x.Group)
                .ToList();
        }

        public async Task<Character> AddOrUpdateCharacter(string characterName, CharacterDTO dto)
        {
            characterName = _marvelApiTasks.NormalizePageName(characterName);

            var character = await GetCharacterByName(characterName);

            if (character == null)
            {
                character = new Character()
                {
                    Name = characterName,
                    FlavorText = dto.FlavorText,
                    PrimaryImage = dto.PrimaryImage
                };
                _marvelDbContext.Characters.Add(character);
            }
            else
            {
                character.Name = dto.Name;
                character.FlavorText = dto.FlavorText;
                character.PrimaryImage = dto.PrimaryImage;
            }
            
            await _marvelDbContext.SaveChangesAsync();

            return character;
        }

        // TODO: use a dto
        public async Task<Group> AddOrUpdateGroup(string groupName, Group dto)
        {
            groupName = _marvelApiTasks.NormalizePageName(groupName);

            var group = await GetGroupByName(groupName);

            if (group == null)
            {
                group = new Group()
                {
                    Name = groupName,
                    FlavorText = dto.FlavorText,
                    PrimaryImage = dto.PrimaryImage
                };
                _marvelDbContext.Groups.Add(group);
            }
            else
            {
                group.Name = dto.Name;
                group.FlavorText = dto.FlavorText;
                group.PrimaryImage = dto.PrimaryImage;
            }

            await _marvelDbContext.SaveChangesAsync();

            return group;
        }
    }
}
