using Oazachaosu.Core.Common;
using System;

namespace WordkiModel
{
    public interface IUser
    {
        long Id { get; set; }
        string Name { get; set; }
        string Password { get; set; }
        string ApiKey { get; set; }
        DateTime CreateDateTime { get; set; }
        bool IsRegister { get; set; }
        DateTime DownloadTime { get; set; }
        TranslationDirection TranslationDirection { get; set; }
        bool AllWords { get; set; }
    }
}