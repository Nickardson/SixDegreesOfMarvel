function getGroupsOfCharacter(characterName, callback) {
    $.ajax({
        url: '/api/marvel/character/groups/?characterName=' + characterName,
        method: 'GET',
        success: callback
    });
}

function getCharactersInGroup(groupName, callback) {
    $.ajax({
        url: '/api/marvel/group/characters/?groupName=' + groupName,
        method: 'GET',
        success: callback
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

function Chain(type, names, selectedName) {
    this.type = type;
    this.names = names;
    this.selectedName = selectedName;
}

var app = new Vue({
    el: '#app',
    data: {
        chains: []
    },
    methods: {
        /**
         * 
         * @param {} type 'Character' or 'Group'
         * @param {} name name of the page i.e. 'Captain America (Steve Rogers)'
         * @returns {} 
         */
        exploreChain: function (type, name) {
            if (type === ChainTypeCharacter) {
                getGroupsOfCharacter(name,
                    function (data) {
                        console.log(data);
                        //pushGroupsRow(data.Groups);
                    });
            }
        }
    }
});

app.chains.push(new Chain(ChainTypeGroup, [
    'Force Works'
], 'Force Works'));
app.chains.push(new Chain(ChainTypeCharacter, [
    'Captain America (Steve Rogers)',
    'Iron Man (Anthony Stark)'
], 'Iron Man (Anthony Stark)'));
app.chains.push(new Chain(ChainTypeGroup, [
    'Avengers'
], 'Avengers'));
