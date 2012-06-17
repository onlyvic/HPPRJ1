jQuery(document).ready(function () {
    startSiteController.init();
});
startSiteController = {
    init: function () {
        //        var username = utility.readCookie(utility.logonCookie.username);
        //        var keyremember = utility.readCookie(utility.logonCookie.keyremember);
        //        if (username != "" && username != null && keyremember != "" && keyremember != null) {
        //            jQuery('body').showLoading();
        //            utility.callServer('app.php/user/getremember', { username: username, keyremember: keyremember }, startSiteController.callBackGetRemember);
        //        }
        //        else {
        //            startSiteController.loadPage();
        //        }
        try {
            startSiteController.loadPage();
        }
        catch (Exception) {
            console.log(Exception);
        }

    },
    callBackGetRemember: function (response) {
        //        days = 10;
        //        utility.setCookie(utility.logonCookie.keyremember, response.Data.keyremember, days);
        //        utility.setCookie(utility.logonCookie.username, response.Data.accountname, days);
        //        utility.setCookie(utility.logonCookie.fullname, response.Data.fullname, days);
        //        jQuery('body').hideLoading();
        //        startSiteController.loadPage();
    },
    loadPage: function () {
        switch (startSiteController.findPage()) {
            case INDEX_PAGE:
                Utility.jsLoader('PageIndexController', 'Web/Pages/Index/Controller/', function () {
                    PageIndexController.init();
                });
                break;
            //            case VIEW_PAGE:        
            //                viewController.init();        
            //                break;        
            //            case SUPPLIER_PAGE:        
            //                supplierController.init();        
            //                break;        
            default:
                Utility.jLoader('PageIndexController', 'Web/Pages/Index/Controller/', function () {
                    PageIndexController.init();
                });
                break;
        };
    },
    findPage: function () {
        var host = window.location.host;
        var hostname = window.location.hostname;
        var path = window.location.pathname;

        if (path.indexOf(INDEX_PAGE) !== -1) {
            return INDEX_PAGE;
        }
        else if (path.indexOf(VIEW_PAGE) !== -1) {
            return VIEW_PAGE;
        }
        else if (path.indexOf(SUPPLIER_PAGE) !== -1) {
            return SUPPLIER_PAGE;
        }
        else {
            return INDEX_PAGE;
        }
    }
}
