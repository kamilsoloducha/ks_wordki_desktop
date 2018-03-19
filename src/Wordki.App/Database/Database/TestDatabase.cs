using NLog;
using Wordki.Models;
using WordkiModel;
using WordkiModel.Extensions;

namespace Wordki.Database.Database
{
    public static class TestDatabase
    {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static void CreateTestDatabase(IDatabase database)
        {
            logger.Debug("Create test database");
            AddEmptyGroup(database);
            AddGroupWith5Results(database);
            AddGroupWith5Words(database);
            database.SaveDatabase();
            logger.Debug("Test database created");
        }

        private static void AddEmptyGroup(IDatabase database)
        {
            logger.Debug("  Add empty group");
            IGroup group = new Group()
            {
                Name = "Empty group"
            };
            database.AddGroup(group);

        }

        private static void AddGroupWith5Words(IDatabase database)
        {
            logger.Debug("  Add group with 5 words");
            IGroup group = new Group()
            {
                Name = "5 words"
            };
            for (int i = 0; i < 5; i++)
            {
                IWord word = new Word()
                {
                    Language1 = i.ToString(),
                    Language2 = i.ToString(),
                };
                group.AddWord(word);
            }
            database.AddGroup(group);
        }

        private static void AddGroupWith5Results(IDatabase database)
        {
            logger.Debug("  Add group with 5 results");
            IGroup group = new Group()
            {
                Name = "5 results"
            };
            for (int i = 0; i < 5; i++)
            {
                IResult result = new Result();
                group.AddResult(result);
            }
            database.AddGroup(group);
        }

    }
}
