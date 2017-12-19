using Moq;
using NUnit.Framework;

using System.Collections.Generic;
using System.Threading.Tasks;
using Wordki.Database;
using Wordki.Models;
using Wordki.ViewModels;
using WordkiModel;

namespace Wordki.Test.Commands.BuilderTests
{
    [TestFixture]
    public class AddGroupTests
    {

        BuilderViewModel viewModel;
        Mock<IDatabase> mock;

        [SetUp]
        public void SetUp()
        {
            mock = new Mock<IDatabase>();
            mock.Setup(o => o.AddGroupAsync(It.IsAny<IGroup>())).Returns(Task.FromResult(true));
            viewModel = new BuilderViewModel();
        }

        [Test]
        public void Add_first_group_test()
        {
            mock.Setup(o => o.Groups).Returns(new List<IGroup>());
            viewModel.Database = mock.Object;
            viewModel.SelectedGroup = null;

            viewModel.AddGroupCommand.Execute(null);

            Assert.NotNull(viewModel.SelectedGroup);
        }

        [Test]
        public void Add_next_group_test()
        {
            IGroup group = new Group();
            mock.Setup(o => o.Groups).Returns(new List<IGroup>());
            viewModel.Database = mock.Object;
            viewModel.SelectedGroup = group;

            viewModel.AddGroupCommand.Execute(null);

            Assert.NotNull(viewModel.SelectedGroup);
            Assert.AreNotSame(group, viewModel.SelectedGroup);
        }

        

    }
}
