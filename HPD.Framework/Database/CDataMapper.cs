using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using HPD.Framework.Log;
namespace HPD.Framework.Database
{
    /// <summary>
    /// Map data from a source into a target object
    /// by copying public property values.
    /// </summary>
    /// <remarks></remarks>
    public static class CDataMapper
    {
        /// <summary>
        /// Sets an object's property with the specified value,
        /// coercing that value to the appropriate type if possible.
        /// </summary>
        /// <param name="target">Object containing the property to set.</param>
        /// <param name="propertyName">Name of the property to set.</param>
        /// <param name="value">Value to set into the property.</param>
        public static void SetPropertyValue(
            object target, string propertyName, object value)
        {
            string className = target.GetType().ToString();
            try
            {
                PropertyInfo propertyInfo = target.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                    return;

                if (value == null)
                    propertyInfo.SetValue(target, value, null);
                else
                {
                    Type pType = GetPropertyType(propertyInfo.PropertyType);
                    Type vType = GetPropertyType(value.GetType());
                    if (pType.Equals(vType))
                    {
                        // types match, just copy value
                        propertyInfo.SetValue(target, value, null);
                    }
                    else
                    {
                        // types don't match, try to coerce
                        if (pType.Equals(typeof(Guid)))
                            propertyInfo.SetValue(target, new Guid(value.ToString()), null);
                        else if (pType.Equals(typeof(byte[])) && vType.Equals(typeof(string)))
                            propertyInfo.SetValue(target, Convert.FromBase64String(value.ToString()), null);
                        else if (pType.IsEnum && vType.Equals(typeof(string)))
                            propertyInfo.SetValue(target, Enum.Parse(pType, value.ToString()), null);
                        else if (value == "" && vType.Equals(typeof(string)))
                            propertyInfo.SetValue(target, null, null);
                        else if (value == DBNull.Value && !pType.Equals(typeof(string)))
                            propertyInfo.SetValue(target, null, null);
                        else
                            propertyInfo.SetValue(target, Convert.ChangeType(value, pType), null);
                    }
                }
            }
            catch (Exception ex)
            {
                string info = string.Format("Database.DataMapper.SetPropertyValue[class:{0} property:{1}]", className, propertyName);
                CLogManager.WriteFW(info, ex.Message);
            }
        }

        public static PropertyInfo[] GetSourceProperties(Type sourceType)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(sourceType);
            foreach (PropertyDescriptor item in props)
                if (item.IsBrowsable)
                    result.Add(sourceType.GetProperty(item.Name));
            return result.ToArray();
        }

        /// <summary>
        /// Returns a property's type, dealing with
        /// Nullable(Of T) if necessary.
        /// </summary>
        /// <param name="propertyType">Type of the
        /// property as returned by reflection.</param>
        private static Type GetPropertyType(Type propertyType)
        {
            Type type = propertyType;
            if (type.IsGenericType &&
                (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return Nullable.GetUnderlyingType(type);
            return type;
        }
    }
}