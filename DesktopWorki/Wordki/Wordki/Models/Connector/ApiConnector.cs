using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Wordki.Helpers;
using Wordki.Helpers.Command;

namespace Wordki.Models.Connector {
  public class ApiConnector {
    public ApiResponse SentRequest(ApiRequest pRequest) {
      Logger.LogInfo("Wysyłam: {0} - {1}", pRequest.Url, pRequest.Message);
      ApiResponse lResponse = new ApiResponse();
      byte[] lData = null;
      HttpWebRequest lRequest;
      try {
        lRequest = (HttpWebRequest)WebRequest.Create(pRequest.Url);
        lRequest.Timeout = 10000;
        lRequest.ReadWriteTimeout = 10000;
      } catch (Exception e) {
        Logger.LogError("Błąd połączenia - {0}", e.Message);
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
          Logger.LogError("Błąd połączenia z serwerem: {0}", exception.Message);
          return new ApiResponse {
            IsError = true,
          };
        }
      }
      HttpWebResponse lWebResponse;
      try {
        lWebResponse = (HttpWebResponse)lRequest.GetResponse();
      } catch (Exception e) {
        Logger.LogInfo("Błąd połączenia z serwerem - {0}", e.Message);
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
      Logger.LogInfo("Odpowiedź: {0}", pMessage);
      ApiResponse lResponse;
      try {
        lResponse = JsonConvert.DeserializeObject<ApiResponse>(pMessage);
      } catch (Exception e) {
        Logger.LogError("GetResponse Error: {0}", e.Message);
        return new ApiResponse {
          IsError = true,
        };
      }
      return lResponse;
    }


    public static CommandQueue<ICommand> GetCommonGroupsQueue() {
      //DateTime downloadDateTime = new DateTime();
      CommandQueue<ICommand> lQueue = new CommandQueue<ICommand>();
      //lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestGetCommonGroups("dupa", 1, 1)) { OnCompleteCommand = Database.GetDatabase().OnReadCommonGroup });
      //lQueue.OnQueueComplete += success => {
      //  if (success) {
      //    User user = Database.GetDatabase().User;
      //    user.DownloadTime = downloadDateTime;
      //    Database.GetDatabase().AddOrUpdateUser(user);
      //  }
      //};
      return lQueue;
    }
  }
}
