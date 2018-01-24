using System;

namespace Wordki.Helpers.WordComparer
{
    [Serializable]
    public class SpaceNotCheck : INotCheck
    {
        public string Convert(string text)
        {
            return text.Replace(" ", "");
        }
    }
}
