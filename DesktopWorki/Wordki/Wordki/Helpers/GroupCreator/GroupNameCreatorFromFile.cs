using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
