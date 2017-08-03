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

//getCharactersInGroup('Frightful Four',
//    function(data) {
//        console.log(data);
//    });

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

//getGroupsOfCharacter('Captain America (Steve Rogers)',
//    function (data) {
//        console.log(data);
//        pushGroupsRow(data.Groups);
//    });

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
        chains: []
    },
    methods: {
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
                    function(data) {
                        // remove all chains above this one
                        self.chains = self.chains.splice(self.chains.indexOf(chain));

                        var chainItems = data.Groups.map(function (group) {
                            return {
                                name: group,
                                // TODO: group should be fleshed out with explored info, etc
                                explored: false
                            }
                        });

                        var groupChain = new Chain(ChainTypeGroup, chainItems);
                        self.chains.unshift(groupChain);

                        self.loadingChain = false;
                    },
                    function(xhr, textStatus) {
                        self.loadingChain = false;
                    });
            } else if (chain.type === ChainTypeGroup) {
                self.loadingChain = true;

                getCharactersInGroup(item.name,
                    function (data) {
                        // remove all chains above this one
                        self.chains = self.chains.splice(self.chains.indexOf(chain));

                        var chainItems = data.Characters.map(function (character) {
                            return {
                                name: character,
                                // TODO: group should be fleshed out with explored info, etc
                                explored: false
                            }
                        });

                        var groupChain = new Chain(ChainTypeCharacter, chainItems);
                        self.chains.unshift(groupChain);

                        self.loadingChain = false;
                    },
                    function (xhr, textStatus) {
                        self.loadingChain = false;
                    });
            }
        }
    }
});

app.chains.push(new Chain(ChainTypeGroup, [
    { name: 'Force Works' }
], 'Force Works'));
app.chains.push(new Chain(ChainTypeCharacter, [
    { name: 'Captain America (Steve Rogers)', explored: true },
    { name: 'Iron Man (Anthony Stark)' }
], 'Iron Man (Anthony Stark)'));
app.chains.push(new Chain(ChainTypeGroup, [
    { name: 'Avengers' }
], 'Avengers'));
app.chains.push(new Chain(ChainTypeCharacter, [
    { name: 'Iron Man (Anthony Stark)' },
    { name: 'Captain America (Steve Rogers)', explored: true },
], 'Iron Man (Anthony Stark)'));