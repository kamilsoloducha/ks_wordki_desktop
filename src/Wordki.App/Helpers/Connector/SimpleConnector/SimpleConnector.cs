using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Wordki.Helpers.Connector
{
    public class SimpleConnector<T> : IConnector<T>
    {
        public IParser<T> Parser { get; set; }

        public T SendRequest(IRequest request)
        {
            T response = default(T);
            byte[] lData = null;
            HttpWebRequest lRequest;
            try
            {
                lRequest = (HttpWebRequest)WebRequest.Create(request.Url);
                lRequest.Timeout = 10000;
                lRequest.ReadWriteTimeout = 10000;
            }
            catch (Exception e)
            {
                LoggerSingleton.LogError("Błąd połączenia - {0}", e.Message);
                return default(T);
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
                catch (WebException exception)
                {
                    LoggerSingleton.LogError("Błąd połączenia z serwerem: {0}", exception.Message);
                    return default(T);
                }
            }
            HttpWebResponse lWebResponse;
            try
            {
                lWebResponse = (HttpWebResponse)lRequest.GetResponse();
            }
            catch (Exception e)
            {
                LoggerSingleton.LogInfo("Błąd połączenia z serwerem - {0}", e.Message);
                return default(T);
            }
            var lResponseStream = lWebResponse.GetResponseStream();
            if (lResponseStream == null)
            {
                response = default(T);
            }
            if (lResponseStream != null)
                response = Parser.Parse(new StreamReader(lResponseStream, Encoding.UTF8).ReadToEnd());
            return (T)response;
        }
    }
}
