using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Helpers.Connector;

namespace Wordki.Test.ConnectorTests
{
    [TestFixture]
    public class ApiResponseParerTests
    {

        static ApiResponse<IEnumerable<string>> response = new ApiResponse<IEnumerable<string>>()
        {
            Code = 10,
            Message = "Message",
            Object = new List<string>() { "1", "1", "1", "1", "1", "1", "1", "1" },
        };

        public ApiResponseParerTests()
        {

        }


        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void Try_to_parse_response()
        {
            var parser = new ApiResponeParser<IEnumerable<string>>();
            var responseFromString = parser.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            Assert.AreEqual(response.Code, responseFromString.Code);
            Assert.AreEqual(response.Message, responseFromString.Message);
            Assert.AreEqual(response.Object.GetType(), responseFromString.Object.GetType());
            Assert.AreEqual(response.Object.Count(), responseFromString.Object.Count());
        }

    }
}
