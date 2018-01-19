using Newtonsoft.Json;
using NUnit.Framework;
using Wordki.Helpers.Connector.Requests;
using Wordki.Helpers.Connector.SimpleConnector;
using Wordki.Models;
using WordkiModel;
using WordkiModel.DTO;

namespace Wordki.Test.ConnectorTests.UserPostTests
{
    [TestFixture]
    public class RegisterUserTests
    {

        IUser user = new User
        {
            Name = "user1",
            Password = "password"
        };

        [Test, Order(10)]
        public void Try_to_register_user()
        {
            string messageBack = new ApiConnector().SendRequest(new PostUsersRequest(user));
            Assert.IsNotEmpty(messageBack, "Answer back is empty!");
            UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(messageBack);
            Assert.IsNotNull(userDTO);
            Assert.AreEqual(user.Name, userDTO.Name);
            Assert.AreEqual(user.Password, userDTO.Password);
            Assert.AreNotEqual(0, userDTO.Id);
            Assert.NotNull(userDTO.ApiKey);
            Assert.IsNotEmpty(userDTO.ApiKey);
        }

        [Test, Order(20)]
        public void Try_to_register_same_user()
        {
            string messageBack = new ApiConnector().SendRequest(new PostUsersRequest(user));
            Assert.IsEmpty(messageBack, "Message is not empty. Something is wrong!");
        }
    }
}
