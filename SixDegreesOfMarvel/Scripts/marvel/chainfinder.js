function getGroupsOfCharacter(characterName, callback, failed) {
    $.ajax({
        url: '/api/marvel/character/groups/?characterName=' + characterName,
        method: 'GET',
        success: callback,
        error: failed
    });
}

function getCharactersInGroup(groupName, callback, failed) {
    $.ajax({
        url: '/api/marvel/group/characters/?groupName=' + groupName,
        method: 'GET',
        success: callback,
        error: failed
    });
}

function getCharacters(callback, failed) {
    $.ajax({
        url: '/api/marvel/character',
        method: 'GET',
        success: callback,
        error: failed
    });
}

function getGroups(callback, failed) {
    $.ajax({
        url: '/api/marvel/group',
        method: 'GET',
        success: callback,
        error: failed
    });
}

function pushGroupsRow(groups) {
    var row = $('<div class="row groupRow"></div>');

    var eachWidth = Math.max(10, Math.floor(100 / groups.length)) + '%';

    groups.forEach(function (group) {
        $('<div></div>')
            .text(group)
            .css({ 'width': eachWidth, 'float': 'left' })
            .appendTo(row);
    });

    $(row).appendTo('#chainContainer');
}

var ChainTypeCharacter = 'Character';
var ChainTypeGroup = 'Group';

function Chain(type, items, selectedName) {
    this.timeRetrieved = Date.now();
    this.type = type;
    this.items = items;
    this.selectedName = selectedName;
}

var app = new Vue({
    el: '#app',
    data: {
        loadingChain: false,
        chains: [],

        characters: [],

        connections: [],

        fromCharacter: '',
        toCharacter: ''
    },
    computed: {
        exploredCharacters: function() {
            return this.characters.filter(function(a) {
                //return a.explored;
                return true;
            }).sort(function(a, b) {
                if (a.name < b.name) {
                    return -1;
                } else if (a.name > b.name) {
                    return 1;
                } else {
                    return 0;
                }
            });
        }
    },
    methods: {
        scrollToTop: function() {
            jQuery('html,body').animate({ scrollTop: 0 }, 500);
        },

        updateCharacterList: function () {
            var self = this;
            getCharacters(function (data) {
                self.characters = [];

                data.forEach(function(character) {
                    self.characters.push({
                        id: character.Id,
                        name: character.Name,
                        explored: character.Explored,
                        flavorText: character.FlavorText,
                        primaryImage: character.PrimaryImage
                    });
                });
            });
        },

        determineConnections: function(from, to) {
            $.ajax({
                url: '/api/marvel/connections?fromCharacter=' + from + '&toCharacter=' + to,
                method: 'GET',
                success: function(data) {
                    app.connections = data;
                },
                error: function() {
                    app.connections = [];
                }
            });
        },

        /**
         * 
         * @param {} type 'Character' or 'Group'
         * @param {} item
         * @returns {} 
         */
        exploreChain: function (chain, item) {
            var self = this;


            console.log('Explore chain ', chain, ', item:', item);
            if (chain.type === ChainTypeCharacter) {
                self.loadingChain = true;

                getGroupsOfCharacter(item.name,
                    function (data) {
                        item.explored = true;

                        // remove all chains above this one
                        self.chains = self.chains.splice(self.chains.indexOf(chain));

                        var chainItems = data.Groups.map(function (group) {
                            return {
                                name: group.Name,
                                explored: group.Explored
                            }
                        });

                        var groupChain = new Chain(ChainTypeGroup, chainItems);
                        self.chains.unshift(groupChain);

                        self.loadingChain = false;

                        self.scrollToTop();
                    },
                    function(xhr, textStatus) {
                        self.loadingChain = false;
                    });
            } else if (chain.type === ChainTypeGroup) {
                self.loadingChain = true;

                getCharactersInGroup(item.name,
                    function (data) {
                        item.explored = true;

                        // remove all chains above this one
                        self.chains = self.chains.splice(self.chains.indexOf(chain));

                        var chainItems = data.Characters.map(function (character) {
                            return {
                                name: character.Name,
                                explored: character.Explored
                            }
                        });

                        var groupChain = new Chain(ChainTypeCharacter, chainItems);
                        self.chains.unshift(groupChain);

                        self.loadingChain = false;

                        self.scrollToTop();
                    },
                    function (xhr, textStatus) {
                        self.loadingChain = false;
                    });
            }
        }
    }
});

app.chains.push(new Chain(ChainTypeGroup, [
    { name: 'Avengers', explored: true }
], 'Avengers'));

app.updateCharacterList();

//app.exploreChain(app.chains[0], app.chains[0].items[0]);
