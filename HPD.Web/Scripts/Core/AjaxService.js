/**
* @author Vic
* 2012-04-26
*/
AjaxService = {
    //static method    
    post: function (serviceName, params, funcCallBack) {
        var me = this;
        if (serviceName !== null && serviceName !== 'undefined' && serviceName !== '') {
            var url = me.getUrl(serviceName);            
            $.ajax({
                type        : 'POST',
                url         : url,
                data        : JSON.stringify(params),
                contentType : "application/json; charset=utf-8",
                dataType    : "json",
                success     : function (data, status, jqXHR) {
                //{Success:true,ErrorInfo:null,Data:{total:10,data:[{id:1,name:happy day site},{id:1,name:happy day site}]}}
                    //var res = jQuery.parseJSON('{"Success":true,"ErrorInfo":null,"Data":{"total":10,"user":[{"id":1,"name":"Vic"},{"id":2,"name":"Tai"}]}}');
                    var res = jQuery.parseJSON('{"Success":true,"ErrorCode":null,"Message":null,"Data":{"total":10,"user":[{"id":1,"name":"Vic"},{"id":2,"name":"Tai"}]}}');
                    //console.log(res);
                                        
                    if (res.Success) {
                        funcCallBack(res.Data)
                    }
                    else {
                        if (typeof res.ErrorInfo !== 'undefined' && res.ErrorInfo !== "" && res.ErrorInfo !== null) {
                            if (typeof res.ErrorInfo.System !== 'undefined' && res.ErrorInfo.System !== "" && res.ErrorInfo.System !== null) {
                                alert(res.ErrorInfo.System);
                                return false;
                            }
                            else {
                                alert('Request Server Fail');
                                return false;
                            }
                        }
                    }
                }
            });
        }
    },
    getUrl: function (params) {
        data = params.split('.');
        switch (data[0]) {
            case RoutingUser.Pattern:
                return URL_DOMAIN + RoutingUser.UrlController + RoutingUser.Services[params];
                break;
            default:
                throw "Can not found routing pattern"; //throw exception
                break;
        }
    }
}

