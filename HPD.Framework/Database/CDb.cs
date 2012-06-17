/* modified original from www.dofactory.com 
 DoFactory.Framework.Data */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.IO;

using HPD.Framework.Log;
namespace HPD.Framework.Database
{
    /// <summary>
    /// Class that manages all lower level ADO.NET data base access.
    /// I have only implemented what I needed; not all posible functionality 
    /// for the best Db helper in this universe:-)
    /// 
    /// GoF Design Patterns: Singleton, Factory, Proxy.
    /// </summary>
    /// <remarks>
    /// This class is a 'swiss army knife' of data access. It handles all the 
    /// database access details and shields its complexity from its clients.
    /// 
    /// The Factory Design pattern is used to create database specific instances
    /// of Connection objects, Command objects, etc.
    /// 
    /// This class is like a Singleton -- it is a static class (Shared in VB) and 
    /// therefore only one 'instance' ever will exist.
    /// 
    /// This class is a Proxy in that it 'stands in' for the actual DbProviderFactory.
    /// </remarks>
    public static class CDb
    {
        #region DB to Object

        public static T Map<T>(string xmlData) where T : new()
        {
            IList<T> list = MapList<T>(xmlData);
            if (list.Count > 0)
                return list[0];
            else
                return new T();

        }

        public static IList<T> MapList<T>(string xmlData) where T : new()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(xmlData));

            IList<T> list = new List<T>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    T obj = new T();
                    foreach (DataColumn col in dt.Columns)
                    {
                        CDataMapper.SetPropertyValue(obj, col.ColumnName, row[col]);
                    }
                    list.Add(obj);
                }
            }

            return list;
        }
        public static T Map<T>(DataTable dataTable) where T : new()
        {
            IList<T> list = MapList<T>(dataTable);
            if (list.Count > 0)
                return list[0];
            else
                return new T();

        }
        public static IList<T> MapList<T>(DataTable dataTable) where T : new()
        {
            DataTable dt = dataTable;
            IList<T> list = new List<T>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    T obj = new T();
                    foreach (DataColumn col in dt.Columns)
                    {
                        CDataMapper.SetPropertyValue(obj, col.ColumnName, row[col]);
                    }
                    list.Add(obj);
                }
            }

            return list;
        }
        public static DataSet ConvertXml2DataTable(string xmlData)
        {
            DataSet ds = new DataSet();
            if (xmlData != null)
            {
                ds.ReadXml(new StringReader(xmlData));
            }
            return ds;
        }

        public static DataSet ToDataset(string xmlData)
        {
            DataSet ds = new DataSet();
            try
            {
                ds.ReadXml(new StringReader(xmlData));
            }
            catch (Exception ex)
            {
                CLogManager.Write("HPD.Framework.Database.ToDataset(string xml)", ex.Message, "Database");
            }
            return ds;
        }
        #endregion
    }
}