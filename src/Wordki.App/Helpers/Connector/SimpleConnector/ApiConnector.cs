using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Wordki.Helpers.Connector.SimpleConnector
{
    public class ApiConnector
    {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Response from api server. It is set when there is some response from server.
        /// In other case response equal null and its mean that something goes wrong
        /// and the whole operation is failed.
        /// </summary>
        private HttpWebResponse response;

        /// <summary>
        /// Return value of operation success. If there is good response from server
        /// true value is return. In another case false is return.
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                if (response != null)
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
                return false;
            }
        }

        public string SendRequest(IRequest request)
        {
            byte[] lData = null;
            HttpWebRequest lRequest;
            try
            {
                lRequest = (HttpWebRequest)WebRequest.Create(request.Url);
                lRequest.Timeout = 60000;
                lRequest.ReadWriteTimeout = 60000;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                response = (HttpWebResponse)lRequest.GetResponse();
            }
            catch (WebException webException)
            {
                response = webException.Response as HttpWebResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return string.Empty;
            }
            if (response == null)
            {
                return string.Empty;
            }
            var lResponseStream = response.GetResponseStream();
            if (lResponseStream != null)
                return new StreamReader(lResponseStream, Encoding.UTF8).ReadToEnd();
            return string.Empty;
        }

    }
}
