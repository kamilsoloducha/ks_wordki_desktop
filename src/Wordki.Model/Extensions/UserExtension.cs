using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordkiModel.Extensions
{
    public static class UserExtension
    {

        public static string GetFormatedDateTime(this IUser user)
        {
            return $"{user.DownloadTime.ToString("yyyy-MM-dd")}T{user.DownloadTime.ToString("HH-mm-ss")}";
        }

    }
}
