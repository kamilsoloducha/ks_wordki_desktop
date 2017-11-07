using System;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;
using Repository.Models;
using Wordki.Database;
using Wordki.Models.Translator;

namespace Wordki.Models.Connector
{
    public abstract class ApiRequest
    {
        public static string Host = ConfigurationManager.AppSettings["apiHost"];
        public string Url { get; protected set; }
        public string Message { get; set; }
        public string Method { get; protected set; }
        public Dictionary<string, string> Headers { get; protected set; }
        protected string DataFormat = "yyyy-MM-ddTHH-mm-ss";
        protected JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
        };

        public abstract void PrepareMessage();

    }

    public class ApiRequestLogin : ApiRequest
    {
        private IUser User { get; set; }
        public ApiRequestLogin(IUser user)
        {
            Url = Host + "login";
            Method = "POST";
            User = user;
        }

        public override void PrepareMessage()
        {
            Message = JsonConvert.SerializeObject(User);
        }
    }

    public class ApiRequestRegister : ApiRequest
    {
        private IUser User { get; set; }
        public ApiRequestRegister(IUser user)
        {
            Url = Host + "register";
            Method = "POST";
            User = user;
        }

        public override void PrepareMessage()
        {
            Message = JsonConvert.SerializeObject(User);
        }
    }

    public class ApiRequestGetGroups : ApiRequest
    {
        public ApiRequestGetGroups(IUser lUser)
        {
            Url = Host + "group/" + lUser.DownloadTime.ToString(DataFormat);
            Method = "GET";
            Headers = new Dictionary<string, string> { { "authorization", lUser.ApiKey } };
        }

        public override void PrepareMessage()
        {
        }
    }

    public class ApiRequestGetWords : ApiRequest
    {
        public ApiRequestGetWords(IUser lUser)
        {
            Url = Host + "word/" + lUser.DownloadTime.ToString(DataFormat);
            Method = "GET";
            Headers = new Dictionary<string, string> { { "authorization", lUser.ApiKey } };
        }

        public override void PrepareMessage()
        {
        }
    }

    public class ApiRequestGetResults : ApiRequest
    {
        public ApiRequestGetResults(IUser lUser)
        {
            Url = Host + "result/" + lUser.DownloadTime.ToString(DataFormat);
            Method = "GET";
            Headers = new Dictionary<string, string> { { "authorization", lUser.ApiKey } };
        }

        public override void PrepareMessage()
        {
        }
    }

    public class ApiRequestPutGroups : ApiRequest
    {
        private IDatabase Database { get; set; }
        public ApiRequestPutGroups(IDatabase database)
        {
            Url = Host + "group";
            Method = "PUT";
            Database = database;
            Headers = new Dictionary<string, string> { { "authorization", UserManagerSingleton.Get().User.ApiKey } };
        }

        public override void PrepareMessage()
        {
            //List<IGroup> groups = Database.GetGroupsToSend();
            //Message = JsonConvert.SerializeObject(groups);
        }
    }

    public class ApiRequestPutWords : ApiRequest
    {
        private IDatabase Database { get; set; }
        public ApiRequestPutWords(IDatabase database)
        {
            Url = Host + "word";
            Method = "PUT";
            Headers = new Dictionary<string, string> { { "authorization", UserManagerSingleton.Get().User.ApiKey } };
            Database = database;
        }

        public override void PrepareMessage()
        {
            //List<IWord> words = Database.GetWordsToSend();
            //Message = JsonConvert.SerializeObject(words);
        }
    }

    public class ApiRequestPutResults : ApiRequest
    {
        private IDatabase Database { get; set; }
        public ApiRequestPutResults(IDatabase database)
        {
            Url = Host + "result";
            Method = "PUT";
            Headers = new Dictionary<string, string> { { "authorization", UserManagerSingleton.Get().User.ApiKey } };
            Database = database;
        }

        public override void PrepareMessage()
        {
            //List<IResult> results = Database.GetResultsToSend();
            //Message = JsonConvert.SerializeObject(results);
        }
    }

    public class ApiRequestGetCommonGroups : ApiRequest
    {
        public ApiRequestGetCommonGroups(string pGroupName, int pOffset, int pLimit)
        {
            Url = Host + string.Format("commonGroup/{0}/{1}/{2}/", pGroupName, pOffset, pLimit);
            Method = "GET";
        }

        public override void PrepareMessage()
        {
        }
    }

    public class ApiRequestGetCommonWords : ApiRequest
    {
        public ApiRequestGetCommonWords(IUser lUser, long pGroupId)
        {
            Url = Host + "commonWords/";
            Method = "GET";
            Message = string.Format("groupId={0}", pGroupId);
            Headers = new Dictionary<string, string> { { "authorization", lUser.ApiKey } };
        }

        public override void PrepareMessage()
        {
        }
    }

    public class ApiRequestGetDateTime : ApiRequest
    {
        private IUser User { get; set; }
        public ApiRequestGetDateTime(IUser user)
        {
            Url = Host + "date/";
            Method = "GET";
            User = user;
            Headers = new Dictionary<string, string> {
                { "authorization", User.ApiKey },
                { "test", "test" }
            };
        }

        public override void PrepareMessage()
        {
        }
    }

    public class ApiRequestPutUser : ApiRequest
    {
        private IUser User { get; set; }
        public ApiRequestPutUser(IUser user)
        {
            Url = Host + "api/user/";
            User = user;
            Method = "PUT";
        }

        public override void PrepareMessage()
        {
            Message = JsonConvert.SerializeObject(User);
        }
    }

    public class TranslationRequest : ApiRequest
    {

        public TranslationRequest(RequestBag bag)
        {
            Url = "https://"+$"glosbe.com/gapi/translate?from={bag.From}&dest={bag.To}&format=json&phrase={bag.Word}";
            Method = "GET";
        }

        public override void PrepareMessage()
        {
        }
    }
}
