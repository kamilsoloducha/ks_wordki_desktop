using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Wordki.Helpers.GroupCreator;

namespace Wordki.Test.Helpers.GroupNameCreatorTests
{
    [TestClass]
    public class GroupNameCreatorFromFileTests
    {

        private IGroupNameCreator creator = new GroupNameCreatorFromFile();
        private string name = "name";

        [TestMethod]
        public void Get_group_name_from_file_only_test()
        {
            string input = $"{name}.txt";
            string output = creator.CreateName(input);
            Assert.AreEqual(name, output);
        }

        [TestMethod]
        public void Get_group_name_from_path_to_file_test()
        {
            string input = Path.Combine("C:\\test", $"{name}.txt");
            string output = creator.CreateName(input);
            Assert.AreEqual(name, output);
        }

        [TestMethod]
        public void Get_group_name_from_group_name_test()
        {
            string input = name;
            string output = creator.CreateName(input);
            Assert.AreEqual(name, output);
        }

    }
}
