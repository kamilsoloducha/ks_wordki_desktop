using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Wordki.ViewModels;
using WordkiModel;

namespace Wordki.Test.IntegratedViewModelTests.BuilderViewModelTests
{
    [TestFixture]
    public class InitViewModelTests
    {

        BuilderViewModel ViewModel { get; set; }
        

        [SetUp]
        public void SetUp()
        {
            ViewModel = new BuilderViewModel();
            ViewModel.Database.Groups.Clear();
        }

        [Test]
        public void Init_view_model_without_groups_test()
        {
            ViewModel.InitViewModel();

            Assert.IsNull(ViewModel.SelectedGroup);
            Assert.IsNull(ViewModel.SelectedWord);
        }

        [Test]
        public void Init_view_model_with_group_without_words_test()
        {
            Utility.WordCount = 0;
            Utility.ResultCount = 0;
            IGroup group = Utility.GetGroup();
            ViewModel.Database.Groups.Add(group);
            ViewModel.InitViewModel();

            Assert.AreSame(group, ViewModel.SelectedGroup);
            Assert.IsNull(ViewModel.SelectedWord);
        }


        [Test]
        public void Init_view_model_with_many_groups_without_words_test()
        {
            Utility.WordCount = 0;
            Utility.ResultCount = 0;
            IEnumerable<IGroup> groups = Utility.GetGroups();
            foreach(IGroup group in groups)
            {
                ViewModel.Database.Groups.Add(group);
            }
            ViewModel.InitViewModel();

            Assert.AreSame(groups.Last(), ViewModel.SelectedGroup);
            Assert.IsNull(ViewModel.SelectedWord);
        }

        [Test]
        public void Init_view_model_with_group_with_word_test()
        {
            Utility.WordCount = 1;
            Utility.ResultCount = 0;
            IGroup group = Utility.GetGroup();
            ViewModel.Database.Groups.Add(group);
            ViewModel.InitViewModel();

            Assert.AreSame(group, ViewModel.SelectedGroup);
            Assert.AreSame(group.Words.Last(), ViewModel.SelectedWord);
        }

    }
}
