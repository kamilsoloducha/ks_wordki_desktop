using System;
using System.Security.Cryptography;
using System.Text;

namespace Wordki.Helpers {

  public class Hash {
    public static string GetMd5Hash(MD5 md5Hash, string input) {
      StringBuilder lBuilder = null;
      try {
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        lBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++) {
          lBuilder.Append(data[i].ToString("x2"));
        }
      } catch (Exception lException) {
        Logger.LogError("{0} - {1}", "Blad w GetMd5Hash", lException.Message);
      }
      if (lBuilder != null)
        return lBuilder.ToString();
      return "";
    }
  }
}
