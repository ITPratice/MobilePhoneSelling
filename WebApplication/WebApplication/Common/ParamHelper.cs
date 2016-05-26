using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication.Models
{
    public class ParamHelper
    {
        static readonly ParamHelper _instance = new ParamHelper();

        public static ParamHelper Instance
        {
            get { return _instance; }
        }

        public ParamHelper() { }

        /// <summary>
        /// Get ID of Table follow new form
        /// </summary>
        /// <param name="oldId"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string GetNewId(string oldId, string prefix)
        {
            if (String.IsNullOrEmpty(oldId)) return prefix + "0001";
            string suffix = oldId.Substring(prefix.Length);
            int id = Int16.Parse(suffix) + 1;
            suffix = "0000" + id;
            return prefix + suffix.Substring(suffix.Length - 4);
        }

        /// <summary>
        /// Get MD5 Text
        /// </summary>
        /// <param name="_text"></param>
        /// <returns></returns>
        public string MD5Hash(string _text)
        {
            MD5 _md5 = new MD5CryptoServiceProvider();

            //compute hash from the byte of text
            _md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(_text));

            //get hash result after compute it
            byte[] _result = _md5.Hash;

            StringBuilder _strBuilder = new StringBuilder();
            for (int i = 0; i < _result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                _strBuilder.Append(_result[i].ToString("x2"));
            }
            return _strBuilder.ToString();
        }
    }
}