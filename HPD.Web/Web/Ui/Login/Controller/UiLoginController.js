UiLoginController = {
    init: function (idDiv) {
        Utility.htmlLoader(idDiv, 'UiLoginView', 'Web/Ui/Login/View/', function () {
            cartItem = { "ID": 123, "Quantity": 2 }
            AjaxService.post('User.GetUserName', cartItem, function (data) {
                //alert("2222");
                //console.log(data);
            });
        });
        //                jQuery('#' + idDiv).load(URL_CLIENT_HTML + 'Web/Ui/Login/View/UiLoginView.html', function () {
        //                    //console.info('4');
        //                    alert('123');
        //                });
    }
};