using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Wordki.Helpers {
  class ObjectSerializer {
    private const string Dictionary = "Data";

    public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false) {
      using (Stream stream = File.Open(Path.Combine(Dictionary, filePath), append ? FileMode.Append : FileMode.Create)) {
        var binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(stream, objectToWrite);
      }
    }

    public static T ReadFromBinaryFile<T>(string filePath, bool deleteAfter) {
      string path = Path.Combine(Dictionary, filePath);
      if (!File.Exists(path)) {
        return default(T);
      }
      T obj;
      using (Stream stream = File.Open(path, FileMode.Open)) {
        var binaryFormatter = new BinaryFormatter();
        obj = (T)binaryFormatter.Deserialize(stream);
      }
      if (deleteAfter) {
        File.Delete(path);
      }
      return obj;
    }

  }
}
