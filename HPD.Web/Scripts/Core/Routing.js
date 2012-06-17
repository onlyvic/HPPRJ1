RoutingUser = {
    Pattern: "User",
    UrlController: URL_CONTROLLER_SERVER+"App/UserModule/Controller/",
    //UrlController: "192.168.1.55/App/UserMod/Controller/",
    Services: {
        "User.GetUserName": 'UserMod.asmx/GetUserName',
        "User.Register": 'UserMod.asmx/Register'
    }	
};
RoutingPartner = {
    Pattern: "Partner",
    UrlController: "App/PartnerMod/Controller/"
}