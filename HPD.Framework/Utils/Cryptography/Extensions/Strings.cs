using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HPD.Framework.Utils
{
    /// <summary>
    /// A collection of common string utility methods
    /// </summary>
    public static class Strings
    {
        public static bool IsEmpty(object input)
        {
            return input == null || Convert.ToString(input).Length == 0;
        }

        public static bool IsEmpty(string input)
        {
            return IsEmpty((Object)input);
        }

        public static bool IsNumeric(object input)
        {
            return Regex.IsExactMatch(CutWhitespace(Convert.ToString(input)), Regex.REGEX_NUMERIC);
        }

        public static bool IsEmail(string input)
        {
            return Regex.IsExactMatch(input, Regex.REGEX_EMAIL);
        }

        //just to take nulls into consideration
        public static string Trim(string input)
        {
            if (IsEmpty(input)) return input;
            return input.Trim();
        }

        public static string CutWhitespace(string input)
        {
            if (IsEmpty(input)) return input;
            return Trim(Regex.Replace(input, @"\s+", " "));
        }

        public static string CutEnd(string input, int length)
        {
            if (IsEmpty(input)) return input;
            if (input.Length <= length) return string.Empty;
            return input.Substring(0, input.Length - length);
        }

        public static string CutStart(string input, int length)
        {
            if (IsEmpty(input)) return input;
            if (input.Length <= length) return string.Empty;
            return input.Substring(length);
        }

        public static string Start(string input, int length)
        {
            if (IsEmpty(input)) return input;
            if (input.Length <= length) return input;
            return input.Substring(0, length);
        }

        public static string End(string input, int length)
        {
            if (IsEmpty(input)) return input;
            if (input.Length <= length) return input;
            return input.Substring(input.Length - length);
        }

        //how many times does a string exist within another string
        public static string[] GetOccurences(string input, string pattern)
        {
            if (IsEmpty(input) || IsEmpty(pattern)) return new string[] { };
            System.Text.RegularExpressions.MatchCollection col = System.Text.RegularExpressions.Regex.Matches(input, pattern);
            string[] colText = new string[col.Count];
            for (int i = 0; i < col.Count; i++)
            {
                colText[i] = col[i].Value;
            }
            return colText;
        }

        public static int OccurenceCount(string input, string pattern)
        {
            return GetOccurences(input, pattern).Length;
        }

        //public static string Combine(string[] input)
        //{
        //    return Combine(input, "");
        //}

        //public static string Combine(string[] input, string seperator)
        //{
        //    StringBuilder sb = new StringBuilder(100);
        //    for(int i = 0; i < input.Length; i++)
        //    {
        //        sb.Append(input[i]);
        //        if (!string.IsNullOrEmpty(seperator) && (input.Length - i > 1))
        //            sb.Append(seperator);
        //    }
        //    return sb.ToString();
        //}

        //public static string ToPaddedNumber(string input, int count)
        //{
        //    if (IsEmpty(input)) return new string('0', count);
        //    if (input.Length >= count) return input;
        //    return new string('0', count - input.Length) + input;
        //}

        public static string XOR(string input, string strKey)
        {
            if (IsEmpty(input)) return input;
            string strEncoded = string.Empty;
            int nKeyIndex = 0;
            for (int i = 0; i < input.Length; i++)
            {
                strEncoded += Convert.ToChar(input[i] ^ strKey[nKeyIndex]);
                nKeyIndex++;
                if (nKeyIndex == strKey.Length) nKeyIndex = 0;
            }
            return strEncoded;
        }

        public static string ToTitleCase(string Input)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Input);
        }

        /// <summary>
        /// returns a friendly name of a string
        /// eg. "SomeUtilsText" becomes "Some Utils Text"
        /// eg2. "BillInvoiceID" becomes "Bill Invoice" if trimIDText = true
        /// </summary>
        /// <param name="input">the input string</param>
        /// <param name="trimIDText">if "ID" text should be cut off the end of the string</param>
        /// <returns>a friendly name string</returns>
        public static string ToFriendlyName(string input, bool trimIDText)
        {
            if (string.IsNullOrEmpty(input)) return input;
            input = input.Trim(); //trim it
            if (input.ToUpper() == input) return input;    //if its all capitals we cant do anything with it
            StringBuilder sb = new StringBuilder(input.Length);
            char? last = null;
            foreach (char c in input.ToCharArray())
            {
                if (last != null && Char.IsUpper(c)) // && Char.IsLower(last ?? char.MinValue))
                    sb.Append(" ").Append(c);
                else sb.Append(c);
                last = c;
            }
            if (trimIDText)
            {
                //if the string ends with ' id' cut it off
                string strOutput = sb.ToString();
                if (strOutput.ToLower().EndsWith(" id"))
                    return CutEnd(strOutput, 3);
            }
            return sb.ToString();
        }
        public static string ToFriendlyName(string input)
        {
            return ToFriendlyName(input, true);
        }

        public static class Regex
        {
            public const string REGEX_EMAIL = @"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,8}|[0-9]{1,8})(\]?)";

            //Matches a positive or negative decimal value with any precision and scale (whole number or decimal). 
            //Allows for left-padded zeroes, commas as group separator, negative sign (-) or parenthesis to indicate negative number.
            public const string REGEX_NUMERIC = @"^\-?\(?([0-9]{0,3}(\,?[0-9]{3})*(\.?[0-9]*))\)?$";

            public const string REGEX_URL = @"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*";

            public const string REGEX_IPADDRESS = @"(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Third>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)";


            //wraps the default regex isMatch to include regex options to pre compile and ignore case. also checks for nulls and empty strings
            public static bool IsExactMatch(string input, string pattern)
            {
                if (IsEmpty(input)) return false;
                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                if (!m.Success) return false;
                return m.Groups[0].Value == input;
            }

            //wraps the default regex Contains to include regex options to pre compile and ignore case. also checks for nulls and empty strings
            public static bool Contains(string input, string pattern)
            {
                if (IsEmpty(input)) return false;
                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(input, pattern);
                return m.Success;
            }

            //wraps the default regex Replace to include regex options to pre compile and ignore case. also checks for nulls and empty strings
            public static string Replace(string input, string pattern, string replace)
            {
                if (IsEmpty(input)) return input;
                return System.Text.RegularExpressions.Regex.Replace(input, pattern, replace, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }

            //pattern must be something like :
            // @"Subject\s*\:\s*(?<SubjectReturn>.*)\r\n"
            //from a string "Subject: Testing", with groupname "SubjectReturn" will return "Testing"
            // the pattern must contain the groupname text ?<AnyGroupName>. in it to return anything
            public static string GetMatch(string input, string pattern, string groupname)
            {
                Match match = System.Text.RegularExpressions.Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Group grp = match.Groups[groupname];
                    if (grp != null)
                        return grp.Value;
                }
                return string.Empty;
            }
        }
    }
}