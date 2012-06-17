using System;
using System.Collections.Generic;
using System.Text;

namespace HPD.BUS.BusinessObjects.Global
{
    [Serializable]
    public class CApplicationMessage : CObjectBase
    {
        public CApplicationMessage()
        {
            CSVFields = new string[] { "ID", "Code", "Name", "Type" };
        }
        #region Public property
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }

        public bool IsSuccessfull
        {
            get
            {
                if (Result == null) return false;
                if (Result.ToString() == "0" || Result.ToString() == "") return false;
                return true;
            }
            set { }
        }

        public object Result { get; set; }

        public string JsonString
        {
            get
            {
                return ToJsonString();
            }
            set { }
        }
        private string ToJsonString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append("{");
            ret.AppendFormat("\"ID\":\"{0}\",\n", ID);
            ret.AppendFormat("\"Code\":\"{0}\",\n", Code);
            ret.AppendFormat("\"Name\":\"{0}\",\n", Name);
            ret.AppendFormat("\"Description\":\"{0}\",\n", Description);
            ret.AppendFormat("\"Type\":\"{0}\",\n", Type);
            ret.AppendFormat("\"Success\":\"{0}\",\n", IsSuccessfull);
            if (Result != null)
            {
                ret.AppendFormat("\"Result\":\"{0}\"\n", Result.ToString());
            }
            else
            {
                ret.AppendFormat("\"Result\":\"{0}\"\n", "");
            }
            ret.Append("}");

            return ret.ToString();
        }
        #endregion
    }
}
