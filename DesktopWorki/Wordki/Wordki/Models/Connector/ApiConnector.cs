using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Wordki.Helpers;

namespace Wordki.Models.Connector {
  public class ApiConnector {
    public ApiResponse SentRequest(ApiRequest pRequest) {
      LoggerSingleton.LogInfo("Wysyłam: {0} - {1}", pRequest.Url, pRequest.Message);
      ApiResponse lResponse = new ApiResponse();
      byte[] lData = null;
      HttpWebRequest lRequest;
      try {
        lRequest = (HttpWebRequest)WebRequest.Create(pRequest.Url);
        lRequest.Timeout = 10000;
        lRequest.ReadWriteTimeout = 10000;
      } catch (Exception e) {
        LoggerSingleton.LogError("Błąd połączenia - {0}", e.Message);
        return new ApiResponse { IsError = true };
      }
      lRequest.Method = pRequest.Method;
      if (pRequest.Headers != null) {
        foreach (KeyValuePair<string, string> lItem in pRequest.Headers) {
          lRequest.Headers.Add(lItem.Key, lItem.Value);
        }
      }
      if (pRequest.Message != null && !pRequest.Message.Equals("")) {
        lData = Encoding.UTF8.GetBytes(pRequest.Message);
        lRequest.ContentLength = lData.Length;
        lRequest.ContentType = "application/x-www-form-urlencoded";
      }
      if (lData != null) {
        try {
          using (var lStreamWriter = lRequest.GetRequestStream()) {
            lStreamWriter.Write(lData, 0, lData.Length);
          }
        } catch (WebException exception) {
          LoggerSingleton.LogError("Błąd połączenia z serwerem: {0}", exception.Message);
          return new ApiResponse {
            IsError = true,
          };
        }
      }
      HttpWebResponse lWebResponse;
      try {
        lWebResponse = (HttpWebResponse)lRequest.GetResponse();
      } catch (Exception e) {
        LoggerSingleton.LogInfo("Błąd połączenia z serwerem - {0}", e.Message);
        return new ApiResponse();
      }
      var lResponseStream = lWebResponse.GetResponseStream();
      if (lResponseStream == null) {
        lResponse = new ApiResponse();
      }
      if (lResponseStream != null)
        lResponse = GetResponse(new StreamReader(lResponseStream, Encoding.UTF8).ReadToEnd());
      return lResponse;
    }

    public ApiResponse GetResponse(string pMessage) {
      LoggerSingleton.LogInfo("Odpowiedź: {0}", pMessage);
      ApiResponse lResponse;
      try {
        lResponse = JsonConvert.DeserializeObject<ApiResponse>(pMessage);
      } catch (Exception e) {
        LoggerSingleton.LogError("GetResponse Error: {0}", e.Message);
        return new ApiResponse {
          IsError = true,
        };
      }
      return lResponse;
    }
  }
}
