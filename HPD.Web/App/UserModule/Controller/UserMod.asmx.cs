using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using HappyDaySystem.App.UserMod.Biz;
using HappyDaySystem.App.CoreMod.Utils;

namespace HappyDaySystem.App.UserMod.Controller
{
    /// <summary>
    /// Summary description for UserMod
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class UserMod : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        
        [WebMethod(Description = "Add item to ShoppingCart")]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public string GetUserName(int ID, int Quantity)
        {
            ServerResponse resp = new ServerResponse();
            NameValueCollection data = new NameValueCollection();
            var dt = new DataTable("fooddata");
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Rows.Add(1, "Hello");
            dt.Rows.Add(2, "World");
            dt.Rows.Add(1, "Foo"); // dupe key

            var dt1 = new DataTable("citydata");
            dt1.Columns.Add("Id", typeof(int));
            dt1.Columns.Add("Name", typeof(string));
            dt1.Rows.Add(1, "hcm");
            dt1.Rows.Add(2, "hn");
            dt1.Rows.Add(1, "cantho"); // dupe key

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.Tables.Add(dt1);

            var nvc = new NameValueCollection();
            foreach (DataRow row in dt.Rows)
            {
                string key = row[0].ToString();
                // tidy up key here
                nvc.Add(key, row[1].ToString());
            }
            resp.setResponseData(nvc);

            
            //resp.setResponseDataset(ds);
            JavaScriptSerializer js = new JavaScriptSerializer();
            string temp  = js.Serialize(resp);
            return temp;
            //return UserBiz.GetUserName(); 


            //string str =// "{Success:true,ErrorInfo:null,Data:{total:10,data:[{id:1,name:happy day site},{id:1,name:happy day site}]}}";

            //return str;
        }
    }
}
