//create aCQueryString from the current URL, add 'id','user' and 'sessionId' values and remove an 'action' value
//output : "?id=123&user=brad&sessionId=ABC"
//string strQuery =CQueryString.Current.Add("id", "123").Add("user", "brad").Add("sessionId", "ABC").Remove("action").ToString();

//take an existing string and replace the 'id' value if it exists (which it does)
//output : "?id=5678&user=tony"
//strQuery = newCQueryString("id=1234&user=tony").Add("id", "5678", true).ToString();

//create aCQueryString from the current URL, add an 'id' value and encrypt the result
//output : "?DhSbRo10vxUjYC5ChMXO5Q%3d%3d=dkxaLXpSg6aeM71fhHJ4ZQ%3d%3d"
//strQuery =CQueryString.Current.Add("id", "123").Encrypt("my key").ToString();

//takes a previousCQueryString value, decrypts it using the same key and gets the 'id' value
//output : "123"
//strQuery = newCQueryString(strQuery).Decrypt("my key")["id"];

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Collections.Specialized;

using HPD.Framework.Utils.Cryptography.Extensions;
namespace HPD.Framework.Utils.Web
{
    /// <summary>
    /// A chainable query string helper class.
    /// Example usage :
    /// string strQuery =CQueryString.Current.Add("id", "179").ToString();
    /// string strQuery = newCQueryString().Add("id", "179").ToString();
    /// </summary>
    public class CQueryString : NameValueCollection
    {
        public CQueryString() { }

        public CQueryString(string queryString)
        {
            FillFromString(queryString);
        }

        public static CQueryString Current
        {
            get
            {
                return new CQueryString().FromCurrent();
            }
        }

        /// <summary>
        /// extracts aCQueryString from a full URL
        /// </summary>
        /// <param name="s">the string to extract theCQueryString from</param>
        /// <returns>a string representing only theCQueryString</returns>
        public string ExtractQuerystring(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (s.Contains("?"))
                    return s.Substring(s.IndexOf("?") + 1);
            }
            return s;
        }

        /// <summary>
        /// returns aCQueryString object based on a string
        /// </summary>
        /// <param name="s">the string to parse</param>
        /// <returns>theCQueryString object </returns>
        public CQueryString FillFromString(string s)
        {
            base.Clear();
            if (string.IsNullOrEmpty(s)) return this;
            foreach (string keyValuePair in ExtractQuerystring(s).Split('&'))
            {
                if (string.IsNullOrEmpty(keyValuePair)) continue;
                string[] split = keyValuePair.Split('=');
                base.Add(split[0],
                    split.Length == 2 ? split[1] : "");
            }
            return this;
        }

        /// <summary>
        /// returns aCQueryString object based on the currentCQueryString of the request
        /// </summary>
        /// <returns>theCQueryString object </returns>
        public CQueryString FromCurrent()
        {
            if (HttpContext.Current != null)
            {
                return FillFromString(HttpContext.Current.Request.QueryString.ToString());
            }
            base.Clear();
            return this;
        }

        /// <summary>
        /// add a name value pair to the collection
        /// </summary>
        /// <param name="name">the name</param>
        /// <param name="value">the value associated to the name</param>
        /// <returns>theCQueryString object </returns>
        public new CQueryString Add(string name, string value)
        {
            return Add(name, value, false);
        }

        /// <summary>
        /// adds a name value pair to the collection
        /// </summary>
        /// <param name="name">the name</param>
        /// <param name="value">the value associated to the name</param>
        /// <param name="isUnique">true if the name is unique within theCQueryString. This allows us to override existing values</param>
        /// <returns>theCQueryString object </returns>
        public CQueryString Add(string name, string value, bool isUnique)
        {
            string existingValue = base[name];
            if (string.IsNullOrEmpty(existingValue))
                base.Add(name, HttpUtility.UrlEncodeUnicode(value));
            else if (isUnique)
                base[name] = HttpUtility.UrlEncodeUnicode(value);
            else
                base[name] += "," + HttpUtility.UrlEncodeUnicode(value);
            return this;
        }

        /// <summary>
        /// removes a name value pair from theCQueryString collection
        /// </summary>
        /// <param name="name">name of theCQueryString value to remove</param>
        /// <returns>theCQueryString object</returns>
        public new CQueryString Remove(string name)
        {
            string existingValue = base[name];
            if (!string.IsNullOrEmpty(existingValue))
                base.Remove(name);
            return this;
        }

        /// <summary>
        /// clears the collection
        /// </summary>
        /// <returns>theCQueryString object </returns>
        public CQueryString Reset()
        {
            base.Clear();
            return this;
        }

        /// <summary>
        /// Encrypts the keys and values of the entireCQueryString acc. to a key you specify
        /// </summary>
        /// <param name="key">the key to use in the encryption</param>
        /// <returns>an encryptedCQueryString object</returns>
        public CQueryString Encrypt(string key)
        {
            CQueryString qs = new CQueryString();
            Utils.Cryptography.Encryption enc = new Utils.Cryptography.Encryption();
            enc.Password = key;
            for (var i = 0; i < base.Keys.Count; i++)
            {
                if (!string.IsNullOrEmpty(base.Keys[i]))
                {
                    foreach (string val in base[base.Keys[i]].Split(','))
                        qs.Add(enc.Encrypt(base.Keys[i]), enc.Encrypt(HttpUtility.UrlDecode(val)));
                }
            }
            return qs;
        }

        /// <summary>
        /// Decrypts the keys and values of the entireCQueryString acc. to a key you specify
        /// </summary>
        /// <param name="key">the key to use in the decryption</param>
        /// <returns>a decryptedCQueryString object</returns>
        public CQueryString Decrypt(string key)
        {
            CQueryString qs = new CQueryString();
            Utils.Cryptography.Encryption enc = new Utils.Cryptography.Encryption();
            enc.Password = key;
            for (var i = 0; i < base.Keys.Count; i++)
            {
                if (!string.IsNullOrEmpty(base.Keys[i]))
                {
                    foreach (string val in base[base.Keys[i]].Split(','))
                        qs.Add(enc.Decrypt(HttpUtility.UrlDecode(base.Keys[i])), enc.Decrypt(HttpUtility.UrlDecode(val)));
                }
            }
            return qs;
        }

        /// <summary>
        /// overrides the default
        /// </summary>
        /// <param name="name"></param>
        /// <returns>the associated decoded value for the specified name</returns>
        public new string this[string name]
        {
            get
            {
                return HttpUtility.UrlDecode(base[name]);
            }
        }

        /// <summary>
        /// overrides the default indexer
        /// </summary>
        /// <param name="index"></param>
        /// <returns>the associated decoded value for the specified index</returns>
        public new string this[int index]
        {
            get
            {
                return HttpUtility.UrlDecode(base[index]);
            }
        }

        /// <summary>
        /// checks if a name already exists within the query string collection
        /// </summary>
        /// <param name="name">the name to check</param>
        /// <returns>a boolean if the name exists</returns>
        public bool Contains(string name)
        {
            string existingValue = base[name];
            return !string.IsNullOrEmpty(existingValue);
        }

        /// <summary>
        /// outputs theCQueryString object to a string
        /// </summary>
        /// <returns>the encodedCQueryString as it would appear in a browser</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (var i = 0; i < base.Keys.Count; i++)
            {
                if (!string.IsNullOrEmpty(base.Keys[i]))
                {
                    foreach (string val in base[base.Keys[i]].Split(','))
                        builder.Append((builder.Length == 0) ? "?" : "&").Append(HttpUtility.UrlEncodeUnicode(base.Keys[i])).Append("=").Append(val);
                }
            }
            return builder.ToString();
        }
    }
}
