URL_DOMAIN              = ''; //http:www.happyday.com
URL_CLIENT_JS           = '';
URL_CLIENT_HTML         = '';
URL_ROOT                = "index.html";
URL_CLIENT_VIEW         = "view.html";
URL_CLIENT_SUPPLIER     = "supplier.html";
URL_CONTROLLER_SERVER   = "";

INDEX_PAGE              = "index.html";
VIEW_PAGE               = "view.html";
SUPPLIER_PAGE           = "supplier.html";


Utility = {
    jsLoader: function (jsName, jsPath, funcCallBack) {
        //eval("if(typeof  jsName === 'undefined'){console.info('gggggggggg');}");
        //khi viet nhu vay jsName se la object can duoc kiem tra co undefined hay ko
        //ma jsName da duoc define la string co value la 'PageController'

        //eval("if(typeof " + jsName + " === 'undefined'){console.info('oooooo');}");
        //khi viet nhu vay jsName se gán gia tri cua nó vào, để hình thành string cho eval
        //cho nen cai gia tri cua jsName se la cai Object can duoc check undefined
        //eval chi run khi object da duoc xac dinh, khac voi object undefined

//        console.info('test');
//        console.log(typeof c);
//        
//        console.log(c == undefined); //->true, neu a chua dc xac dinh, thi == se define luon a la undefined
//        console.log(eval("c  == undefined")); //object is not define
//        
//        //console.log(b === undefined);//so sanh va ko define object b la undefine
//        //console.log(eval("b  == undefined")); //object is not define
//        console.log(typeof c);
//        console.log(c == undefined); //->false
//        console.log(eval("c  == undefined")); //object is not define

//        var c;
//        console.log(typeof a);
        //        console.log(a);

        eval("if(typeof " + jsName + " === 'undefined'){" +
                "$.getScript(URL_CLIENT_JS + jsPath + jsName + '.js', function () {" +
                "funcCallBack();" +
            "});}else{" +
            jsName + ".init();}");
        
    },
    htmlLoader: function (idObject, htmlName, htmlPath, funcCallBack) {
        jQuery('#' + idObject).load(URL_CLIENT_HTML + htmlPath + htmlName + '.html', function () { funcCallBack(); });
    }
};