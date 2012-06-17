using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

namespace HPD.BUS.BusinessObjects.Global
{
    [Serializable]
    public class CObjectBase
    {
        #region "   XML   "

        public string ExtendProperties { get; set; }

        [XmlInclude(typeof(string))]
        private object GetValueByPropertiesName(string pPropertiesName)
        {
            PropertyInfo info = this.GetType().GetProperty(pPropertiesName);
            if (info == null) return null;
            object obj = info.GetValue(this, null);
            return obj;
        }

        [XmlIgnore]
        public object this[string propertyName]
        {
            get
            {
                PropertyInfo info = this.GetType().GetProperty(propertyName);
                if (info == null) return GetXmlNodeValue(propertyName);
                object obj = info.GetValue(this, null);
                return obj;
            }
        }

        [XmlInclude(typeof(string))]
        private object GetXmlNodeValue(string nodeName)
        {
            if (string.IsNullOrEmpty(ExtendProperties))
            {
                return null;
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(ExtendProperties);
            XmlNodeList nodelist = doc.ChildNodes;
            try
            {
                if (nodelist.Count > 0 && nodelist[0].Attributes.Count > 0)
                {
                    return nodelist[0].Attributes[nodeName].Value;
                }

                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                doc = null;
            }
        }

        public static string ToXML<T>(IList<T> list, CPaging pPaging) where T : CObjectBase
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Root>");
            for (int j = 0; j < list.Count; j++)
            {
                sb.Append("<");
                sb.Append(typeof(T).Name);
                sb.Append(" ");
                PropertyInfo[] properties = list[j].GetType().GetProperties();
                foreach (PropertyInfo p in properties)
                {
                    if (p.Name == "Item") continue;
                    object obj = p.GetValue(list[j], null);
                    if (obj != null && !obj.Equals((decimal)0) && !obj.Equals((int)0))
                    {
                        sb.Append(p.Name);
                        if (obj is DateTime)
                        {
                            sb.AppendFormat("=\"{0}\" ", ((DateTime)obj).ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            sb.AppendFormat("=\"{0}\" ", obj);
                        }
                    }
                }
                sb.Append("/>");
            }
            sb.Append("</Root>");
            return sb.ToString();
        }

        #endregion

        #region "   CSV   "

        [XmlIgnore]
        protected string[] CSVFields = null;

        public virtual string ToCSV(string Separator = ";")
        {
            if (CSVFields == null || CSVFields.Length == 0)
                return "";
            string csvReturn = "", stringTemplate = "\"{0}\"" + Separator;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < CSVFields.Length; i++)
            {
                PropertyInfo pro = this.GetType().GetProperty(CSVFields[i]);
                if (pro != null)
                {
                    object obj = pro.GetValue(this, null);
                    if (obj is DateTime)
                    {
                        sb.AppendFormat(stringTemplate, ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (obj is string)
                    {
                        sb.Append(Newtonsoft.Json.JsonConvert.SerializeObject(obj.ToString()));
                    }
                    else
                    {
                        sb.Append(obj);
                    }
                }
            }
            csvReturn = sb.ToString();
            if (csvReturn.EndsWith(Separator))
                csvReturn = csvReturn.Remove(csvReturn.Length - 1);
            return csvReturn;
        }

        public static string ToCSV<T>(IList<T> list, string Separator = ";", bool isContentHeader = true) where T : CObjectBase
        {
            string csvReturn = "";
            StringBuilder sb = new StringBuilder();
            if (isContentHeader)
            {
                if (list == null || list.Count == 0 || (list.Count > 0 && list[0].CSVFields == null))
                {
                    PropertyInfo[] properties = typeof(T).GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        csvReturn += properties[i].Name + Separator;
                    }
                    if (csvReturn.EndsWith(Separator))
                        csvReturn = csvReturn.Remove(csvReturn.Length - 1);
                }
                else
                {
                    string[] properties = list[0].CSVFields;
                    for (int i = 0; i < properties.Length; i++)
                    {
                        csvReturn += properties[i] + Separator;
                    }
                    if (csvReturn.EndsWith(Separator))
                        csvReturn = csvReturn.Remove(csvReturn.Length - 1);
                }
                if (!string.IsNullOrEmpty(csvReturn))
                    csvReturn += "\n";
            }
            sb.Append(csvReturn);
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append((list[i]).ToCSV(Separator)).Append("\n");
            }
            csvReturn = sb.ToString();
            if (csvReturn.EndsWith("\n"))
                csvReturn = csvReturn.Remove(csvReturn.Length - 1);
            return csvReturn;
        }

        #endregion

        #region "   TOSTRING   "

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public string ToXML()
        {
            StringBuilder xmlReturn = new StringBuilder();
            xmlReturn.Append("<Root>");
            xmlReturn.Append("<");
            xmlReturn.Append(this.GetType().Name);
            xmlReturn.Append(" ");
            PropertyInfo[] properties = this.GetType().GetProperties();
            foreach (PropertyInfo p in properties)
            {
                if (p.Name == "Item")
                    continue;
                object obj = p.GetValue(this, null);
                if (obj != null && !obj.Equals((decimal)0) && !obj.Equals((int)0))
                {
                    try
                    {
                        xmlReturn.Append(p.Name);

                        if (obj is DateTime)
                        {
                            xmlReturn.AppendFormat("=\"{0}\" ", ((DateTime)obj).ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            xmlReturn.AppendFormat("=\"{0}\" ", obj);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            xmlReturn.Append("/>");
            xmlReturn.Append("</Root>");
            return xmlReturn.ToString();
        }

        #endregion

        #region Function

        public static string FormatDate(DateTime? date, string outFormat)
        {
            string str = "";
            if (date == null)
                str = "";
            else
            {
                str = ((DateTime)date).ToString(outFormat);
            }
            return str;
        }

        #endregion
    }
}
