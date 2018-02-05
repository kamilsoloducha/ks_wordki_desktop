﻿using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class GetGroupsRequest : ApiRequestBase
    {

        private string path;
        protected override string Path { get { return path; } }

        public GetGroupsRequest(IUser user) : base(user)
        {
            Method = "GET";
            path = $"Groups/{user.DownloadTime}/{user.ApiKey}";
        }
    }
}
