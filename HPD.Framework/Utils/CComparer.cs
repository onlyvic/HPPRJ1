using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace PMSA.Framework.Utils
{
    public class CComparer<T>:IComparer<T>
    {
        public string Column = "";
        public bool IsDESC = false;

        public CComparer()
        {
        }
        public CComparer(string sortColum,string sortType)
        {
            Column = sortColum;
            if (sortType.ToLower().Equals("desc"))
            {
                IsDESC = true;
            }
        }
        public int Compare(T x, T y)
        {
            PropertyInfo pf = x.GetType().GetProperty(Column);
            if (pf == null) return 0;

            object obj1 ;
            object obj2;

            if (IsDESC)
            {
                 obj1 = pf.GetValue(y, null);
                 obj2 = pf.GetValue(x, null);
            }
            else
            {
                obj1 = pf.GetValue(x, null);
                obj2 = pf.GetValue(y, null);
            }
            if (pf.PropertyType.Equals(typeof(Int64)))
            {
                int t = ((Int64)obj1).CompareTo((Int64)obj2);
                return ((Int64)obj1).CompareTo((Int64)obj2);
            }
            if (pf.PropertyType.Equals(typeof(int)))
            {
                int t = ((int)obj1).CompareTo((int)obj2);
                return ((int)obj1).CompareTo((int)obj2);
            }
            if (pf.PropertyType.Equals(typeof(decimal)))
            {
                return ((decimal)obj1).CompareTo((decimal)obj2);
            }
            if (pf.PropertyType.Equals(typeof(string)))
            {
                return ((string)obj1).CompareTo((string)obj2);
            }
            if (pf.PropertyType.Equals(typeof(DateTime)))
            {
                return ((DateTime)obj1).CompareTo((DateTime)obj2);
            }
            return 0;
        }

    }
}
