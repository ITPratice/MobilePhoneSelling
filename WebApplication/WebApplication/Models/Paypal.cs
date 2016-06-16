using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using WebApplication.Common;

namespace WebApplication.Models
{
    public class Paypal
    {
        public string GetPayPalResponse(string tx)
        {
            try
            {
                string authToken = Constants.TOKEN;
                string txToken = tx;
                string query = string.Format("cmd=_notify-synch&tx={0}&at={1}", txToken, authToken);
                string url = Constants.URL_SUBMIT_PAYMENT;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = query.Length;
                StreamWriter outStreamWriter =
                new StreamWriter(req.GetRequestStream(), Encoding.ASCII);
                outStreamWriter.Write(query);
                outStreamWriter.Close();
                StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream());
                string strResponse = reader.ReadToEnd();
                reader.Close();
                if (strResponse.StartsWith("SUCCESS"))
                    return strResponse;
                else
                    return String.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}