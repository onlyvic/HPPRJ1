UiUserListController = {
    init: function (idDiv) {
        Utility.htmlLoader(idDiv, 'UiUserListView', 'Web/Ui/User/View/', function () {
            console.info(1);
            var movies = [
                        { Name: "The Red Violin", ReleaseYear: "1998" },
                        { Name: "Eyes Wide Shut", ReleaseYear: "1999" },
                        { Name: "The Inheritance", ReleaseYear: "1976" }
                        ];
            $("#movieTemplate").tmpl(movies).appendTo("#movieList");
        });
        
    }
};