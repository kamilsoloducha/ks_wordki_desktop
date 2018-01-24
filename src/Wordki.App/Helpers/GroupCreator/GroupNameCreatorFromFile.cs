using System.IO;

namespace Wordki.Helpers.GroupCreator
{
    public class GroupNameCreatorFromFile : IGroupNameCreator
    {
        public string CreateName(string input)
        {
            return Path.GetFileNameWithoutExtension(input);
            
        }
    }
}
