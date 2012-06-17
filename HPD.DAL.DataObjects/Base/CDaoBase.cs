using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using HPD.Framework.Log;
using HPD.Framework.Database;
using System.Diagnostics;
using HPD.BUS.BusinessObjects.Global;

namespace HPD.DAL.DataObject.Base
{
    public class CDaoBase
    {
        #region Configuration

        protected static string sConnectionString = "";
        protected SqlConnection conn = null;
        protected SqlCommand command;
        protected SqlDataAdapter adapter;
        private const string CALLFUNCTIONSTORENAME = "core_Usp_SYS_VRF_CallFunction";

        #endregion

        #region Public

        public CDaoBase()
        {
            conn = new SqlConnection(sConnectionString);
        }

        private void OpenConnection()
        {

            if (conn == null) conn = new SqlConnection(sConnectionString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        private void CloseConnection()
        {
            if (conn == null) conn = new SqlConnection(sConnectionString);
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
            }
        }

        #endregion

        #region Execute function

        protected DataSet CallFunction(int functionID, string inputValue)
        {
            try
            {
                command = new SqlCommand(CALLFUNCTIONSTORENAME, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@InputValue", inputValue);
                command.Parameters.AddWithValue("@FunctionID", functionID);

                adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                string stack = GetStackTrace(new StackTrace());
                CLogManager.WriteDAL("CallFunction", stack + "::::" + ex.Message);
                throw ex;
            }
        }

        protected IList<T> CallFunctionWithList<T>(int functionID, string inputValue) where T : new()
        {
            try
            {
                DataSet ds = CallFunction(functionID, inputValue);
                if (ds == null) return null;
                if (ds.Tables.Count == 0) return null;
                return CDb.MapList<T>(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                string stack = GetStackTrace(new StackTrace());
                CLogManager.WriteDAL("CallFunctionWithList:IList(1,2)", stack + "::::" + ex.Message);
            }
            return null;
        }
        protected IList<T> CallFunctionWithList<T>(int functionID, string inputValue, ref COutputValue output) where T : new()
        {
            try
            {
                DataSet ds = CallFunction(functionID, inputValue);
                if (ds == null) return null;
                if (ds.Tables.Count == 0) return null;

                IList<T> list = CDb.MapList<T>(ds.Tables[0]);
                if (ds.Tables.Count > 1)
                    output = CDb.Map<COutputValue>(ds.Tables[1]);
                return list;
            }
            catch (Exception ex)
            {
                output = new COutputValue() { Code = "SYSTEM_ERROR", Name = "Error", Description = ex.Message };
            }
            return null;
        }
        protected T CallFunction<T>(int functionID, string inputValue) where T : new()
        {
            try
            {
                DataSet ds = CallFunction(functionID, inputValue);
                if (ds == null) return default(T);
                if (ds.Tables.Count == 0) return default(T);

                return CDb.Map<T>(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                if (typeof(T) == typeof(CApplicationMessage))
                {
                    return CDb.Map<T>("<Message Code=\"SYSTEM_ERROR\" Name = \"Unknow Error\" Result=\"0\" Description = \"" + ex.Message + "\" />");
                }
            }
            return default(T);
        }

        protected string GetStackTrace(StackTrace trace)
        {
            string stack = "";
            for (int i = trace.FrameCount - 1; i >= 0; i--)
            {
                string methodName = trace.GetFrame(i).GetMethod().Name;
                string moduleName = trace.GetFrame(i).GetMethod().ReflectedType.Name;
                stack = stack + "/" + moduleName + "." + methodName;
            }
            return stack;
        }

        #endregion
    }
}
