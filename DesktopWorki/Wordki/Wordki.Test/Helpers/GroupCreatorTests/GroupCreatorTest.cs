using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wordki.Helpers.GroupCreator;
using Wordki.Models;

namespace Wordki.Test.Helpers.GroupCreatorTests
{
    [TestClass]
    public class GroupCreatorTest
    {
        private IGroupCreator groupCreator;

        [TestInitialize]
        public void Init()
        {
            GroupCreatorSettings settings = new GroupCreatorSettings
            {
                WordSeparator = '|',
                ElementSeparator = ';'
            };

            Mock<IFileLoader> mockFileLoader = new Mock<IFileLoader>();
            mockFileLoader.Setup(x => x.LoadFile("")).Returns("");

            Mock<IGroupNameCreator> mockNameCreator = new Mock<IGroupNameCreator>();
            mockNameCreator.Setup(x => x.CreateName("")).Returns("");

            groupCreator = new GroupCreatorFromFile("")
            {
                FileLoader = mockFileLoader.Object,
                GroupNameCreator = mockNameCreator.Object,
                Settings = settings
            };
        }

        [TestMethod]
        public void Try_create_group_from_empty_file_get_empty_name_test()
        {
            Group result = groupCreator.Create();

            Assert.IsTrue(result.Name.Equals(""), $"Group name: {result.Name}");
            
        }

    }
}
