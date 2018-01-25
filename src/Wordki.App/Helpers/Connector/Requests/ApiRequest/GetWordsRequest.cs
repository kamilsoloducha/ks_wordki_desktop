﻿using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class GetWordsRequest : ApiRequestBase
    {
        private string path;
        protected override string Path { get { return path; } }

        public GetWordsRequest(IUser user) : base(user)
        {
            Method = "GET";
            path = $"Words/1990-01-01/{user.ApiKey}";
        }
    }
}
