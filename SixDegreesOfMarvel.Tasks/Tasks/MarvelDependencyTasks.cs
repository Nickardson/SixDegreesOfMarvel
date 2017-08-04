using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        private readonly MarvelApiTasks _marvelApiTasks;

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

            // re-seed the values
            new MarvelDbInitializer().CreateMarvelChacacters(_marvelDbContext);

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
            return await _marvelDbContext.Characters.FirstOrDefaultAsync(
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
            return await _marvelDbContext.Groups.FirstOrDefaultAsync(
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

        //public void BreadthFirstSearch(Character root, Character target)
        //{
        //    var s = new List<Character>();
        //    var q = new Queue<Character>();

        //    s.Add(root);
        //    q.Enqueue(root);

        //    while (q.Any())
        //    {
        //        var current = q.Dequeue();

        //        if (current.Name.Equals(target.Name, StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            return;
        //        }

        //        var adjacent = current.CharacterGroups
        //            // get all characters in all groups this character is in
        //            .SelectMany(x => x.Group.CharacterGroups.Select(y => y.Character))
        //            // except this character itself
        //            .Except(new [] { current });

        //        foreach (var node in adjacent)
        //        {
        //            if (s.All(seenNode => seenNode.CharacterId != node.CharacterId))
        //            {
        //                s.Add(node);
        //                q.Enqueue(node);
        //            }
        //        }
        //    }
        //}

        public List<Tuple<Character, Group>> BreadthFirstSearch(Character root, Character target)
        {
            // Key = From node, Value = To node, and how we got there.
            var connections = new Dictionary<Character, Tuple<Character, Group>>();

            var s = new List<Character>();
            var q = new Queue<Character>();

            s.Add(root);
            q.Enqueue(root);

            while (q.Any())
            {
                var current = q.Dequeue();

                if (current.Name.Equals(target.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
                
                // on the character's connections
                var adjacent = current.CharacterGroups
                    // get the groups the character is directly involved in
                    .Select(directConnection => directConnection.Group)
                    // get all connections, i.e. tuples of (each character in each of those groups) and (the group with which they relate to the character)
                    .SelectMany(group => group.CharacterGroups)
                    // except for any connection which leads directly back to the root character.
                    .Where(connection => connection.Character.CharacterId != current.CharacterId);

                foreach (var node in adjacent)
                {
                    if (s.Any(seenNode => seenNode.CharacterId == node.Character.CharacterId))
                    {
                        continue;
                    }

                    s.Add(node.Character);
                    q.Enqueue(node.Character);

                    connections[node.Character] = new Tuple<Character, Group>(current, node.Group);
                }
            }

            var path = new List<Tuple<Character, Group>>();

            var lookAt = target;
            while (!lookAt.Equals(root))
            {
                if (!connections.ContainsKey(lookAt))
                {
                    throw new NullReferenceException($"Could not find a connection from {lookAt.Name}.");
                }

                var c = connections[lookAt];
                path.Add(new Tuple<Character, Group>(lookAt, c.Item2));
                lookAt = c.Item1;
            }

            path.Add(new Tuple<Character, Group>(root, null));
            path.Reverse();

            return path;
        }

        //public List<ChainLink> FindLinkBetween(Character a, Character b)
        //{
        //    var stack = new Stack<Character>();
        //    FindLinksBetween(a, b, stack);
        //    return stack.Select(x => new ChainLink()
        //    {
        //        Name = x.Name
        //    }).ToList();
        //}

        //public bool FindLinksBetween(Character startCharacter, Character endCharacter, Stack<Character> chain)
        //{
        //    var characters = _marvelDbContext.Characters;

        //    var relatedCharacters = startCharacter.CharacterGroups.Select(x => x.Character);
        //    foreach (var character in relatedCharacters)
        //    {
        //        if (character.Name.Equals(endCharacter.Name, StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            return true;
        //        }

        //        // TODO: check chain

        //        chain.Push(character);

        //        // if the sub-call returns true, we are true too, and no need to pop off the chain
        //        if (FindLinksBetween(character, endCharacter, chain))
        //        {
        //            return true;
        //        }

        //        chain.Pop();
        //    }

        //}
    }
}
