PageIndexController = {
    init: function () {
        Utility.htmlLoader('BodyID', 'PageIndexView', 'Web/Pages/Index/View/', function () {
            //            jQuery('#LoginUI').load(URL_CLIENT_HTML + 'Web/Ui/Login/View/UiLoginView.html', function () {
            //                console.info('4');
            //                alert('123');
            //            });
            PageIndexController.loadUILogin();
        });
    },
    loadUILogin: function () {
        //console.info('1');        
        Utility.jsLoader('UiLoginController', 'Web/Ui/Login/Controller/', function () {
            //console.info('2');                
            UiLoginController.init('Page_Index_LoginUI');
        });

        Utility.jsLoader('UiUserListController', 'Web/Ui/User/Controller/', function () {
            console.info('2');
            UiUserListController.init('Page_Index_UserList');
        });
    }
};