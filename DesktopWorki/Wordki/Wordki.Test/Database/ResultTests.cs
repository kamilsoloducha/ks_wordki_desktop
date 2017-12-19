using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models;
using Wordki.Database;

using Wordki.Database.Repositories;
using WordkiModel;
using WordkiModel.Enums;

namespace Resultki.Test.Database
{
    [TestFixture]
    public class ResultTests
    {
        private IResult result;
        private IGroup group;
        private IGroupRepository groupRepo;
        private IResultRepository resultRepo;

        [SetUp]
        public void SetUp()
        {
            NHibernateHelper.ResetSession();
            NHibernateHelper.ClearDatabase();
            group = new Group()
            {
                Id = 1,
                Name = "test",
                Language1 = LanguageType.English,
                Language2 = LanguageType.Germany,
                State = 1,
                UserId = 1,
            };
            result = new Result()
            {
                Id = 1,
                Accepted = 10,
                Wrong = 10,
                Correct = 10,
                DateTime = DateTime.Now,
                Invisibilities = 10,
                LessonType = LessonType.FiszkiLesson,
                TimeCount = 10,
                TranslationDirection = TranslationDirection.FromFirst,
                State = 1,
                UserId = 1,
            };

            groupRepo = new GroupRepository();
            resultRepo = new ResultRepository();
        }

        [TearDown]
        public void TearDown()
        {
            NHibernateHelper.ClearDatabase();
        }

        [Test]
        public void Save_result_to_database_test()
        {
            groupRepo.Save(group);
            group.Results.Add(result);
            result.Group = group;
            groupRepo.Update(group);

            IGroup groupFromDatabase = groupRepo.Get(group.Id);

            Assert.AreEqual(1, groupFromDatabase.Results.Count);
        }

        [Test]
        public void Save_result_with_group_at_once_test()
        {
            group.Results.Add(result);
            result.Group = group;
            groupRepo.Save(group);

            IGroup groupFromDatabase = groupRepo.Get(group.Id);

            Assert.AreEqual(1, groupFromDatabase.Results.Count);
        }

        [Test]
        public void Save_result_by_result_repository_test()
        {
            groupRepo.Save(group);
            result.Group = group;
            resultRepo.Save(result);

            IGroup groupFromDatabase = groupRepo.Get(group.Id);

            Assert.AreEqual(1, groupFromDatabase.Results.Count, $"Result is not added");

            IResult resultFromDatabase = groupFromDatabase.Results[0];
            Assert.AreEqual(result.Group.Id, resultFromDatabase.Group.Id, $"Wrong groupId after read from database");
        }

        [Test]
        public void Save_result_without_group_test()
        {
            Assert.Throws<NHibernate.PropertyValueException>(() => resultRepo.Save(result));
        }

        [Test]
        public void Get_result_by_groupRepo_test()
        {
            group.Results.Add(result);
            result.Group = group;
            groupRepo.Save(group);
            IResult resultFromDatabase = groupRepo.Get(group.Id).Results.First();
            Assert.AreEqual(result, resultFromDatabase);
        }

        [Test]
        public void Get_result_by_resultRepo_test()
        {
            group.Results.Add(result);
            result.Group = group;
            groupRepo.Save(group);
            IResult resultFromDatabase = resultRepo.Get(result.Id);
            CheckResultEquality(result, resultFromDatabase);
        }

        [Test]
        public void Update_result_by_groupRepo_test()
        {
            group.Results.Add(result);
            result.Group = group;
            groupRepo.Save(group);
            ChangeResult(result);
            groupRepo.Update(group);
            IResult resultFromDatabase = resultRepo.Get(result.Id);
            CheckResultEquality(result, resultFromDatabase);
        }

        [Test]
        public void Update_result_by_resultRepo_test()
        {
            group.Results.Add(result);
            result.Group = group;
            groupRepo.Save(group);
            ChangeResult(result);
            resultRepo.Update(result);
            IResult resultFromDatabase = resultRepo.Get(result.Id);
            CheckResultEquality(result, resultFromDatabase);
        }

        [Test]
        public void Delete_result_by_resultRepo_test()
        {
            group.Results.Add(result);
            result.Group = group;
            groupRepo.Save(group);
            resultRepo.Delete(result);
            Assert.AreEqual(0, resultRepo.RowCount());
        }

        [Test]
        public void Delete_result_by_groupRepo_test()
        {
            group.Results.Add(result);
            result.Group = group;
            groupRepo.Save(group);
            groupRepo.Delete(group);
            Assert.AreEqual(0, resultRepo.RowCount());
        }

        

        private void ChangeResult(IResult result)
        {
            result.Accepted = 20;
            result.Wrong = 20;
            result.Correct = 20;
            result.DateTime = DateTime.Now;
            result.Invisibilities = 20;
            result.LessonType = LessonType.IntensiveLesson;
            result.TimeCount = 20;
            result.TranslationDirection = TranslationDirection.FromSecond;
        }

        private void CheckResultEquality(IResult resultExpected, IResult resultActual)
        {
            Assert.NotNull(resultExpected.Group, "Group in result is null");
            Assert.NotNull(resultActual.Group, "Group in result is null");
            Assert.AreEqual(resultExpected, resultActual, $"Results are not equal");
        }
    }
}
