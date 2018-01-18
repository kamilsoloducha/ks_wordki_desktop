using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Wordki.Helpers.Connector.SimpleConnector
{
    public class ApiConnector
    {

        public HttpWebResponse Response { get; private set; }

        public string SendRequest(IRequest request)
        {
            byte[] lData = null;
            HttpWebRequest lRequest;
            try
            {
                lRequest = (HttpWebRequest)WebRequest.Create(request.Url);
                lRequest.Timeout = 10000;
                lRequest.ReadWriteTimeout = 10000;
            }
            catch (Exception)
            {
                return string.Empty;
            }
            lRequest.Method = request.Method;
            if (request.Headers != null)
            {
                foreach (KeyValuePair<string, string> lItem in request.Headers)
                {
                    lRequest.Headers.Add(lItem.Key, lItem.Value);
                }
            }
            if (request.Message != null && !request.Message.Equals(""))
            {
                lData = Encoding.UTF8.GetBytes(request.Message);
                lRequest.ContentLength = lData.Length;
                lRequest.ContentType = "application/json";
            }
            if (lData != null)
            {
                try
                {
                    using (var lStreamWriter = lRequest.GetRequestStream())
                    {
                        lStreamWriter.Write(lData, 0, lData.Length);
                    }
                }
                catch (WebException)
                {
                    return string.Empty;
                }
            }
            try
            {
                Response = (HttpWebResponse)lRequest.GetResponse();
            }
            catch (Exception)
            {
                return string.Empty;
            }
            var lResponseStream = Response.GetResponseStream();
            if (lResponseStream != null)
                return new StreamReader(lResponseStream, Encoding.UTF8).ReadToEnd();
            return string.Empty;
        }

    }
}
