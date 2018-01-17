using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Util.Collections;
using Wordki.Helpers.Connector;
using Wordki.Helpers.Connector.Requests;
using Wordki.Models;
using WordkiModel;

namespace Wordki.Test
{
    [TestFixture]
    public class Test
    {

        [Test]
        public void test1()
        {
            IUser user = new User()
            {
                ApiKey = "1",
                DownloadTime = new DateTime(1990, 1, 1),
            };
            IRequest request = new GetWordsRequest(user);
            //IConnector<ApiResponse<List<Word>>> connector = new SimpleConnector<ApiResponse<List<Word>>>();
            //connector.Parser = new ApiResponeParser<List<Word>>();
            //ApiResponse<List<Word>> response = null;
            //response = connector.SendRequest(request);

        }

        [Test]
        public void test2()
        {
            var list = new object[] { new object(), new object(), new object(), new object(), new object(), new object() };
            Assert.AreSame(list[0], list.Previous(list[1]));
            Assert.AreSame(null, list.Previous(list[0]));
            Assert.AreSame(list[3], list.Next(list[2]));
            Assert.AreSame(null, list.Next(list.Last()));
        }

        public Task<int> GetAsync()
        {
            return Task.Run<int>(() =>
            {
                Console.WriteLine("Before Delay");
                Task.Delay(1000);
                Console.WriteLine("After Delay");
                return 5;
            });
        }

    }
}
