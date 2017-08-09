using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace Wordki.Helpers {
  public class Serializer {
    public void Serialize(object pObjectToSerialize, string pFilePath) {
      try {
        using (Stream lStream = File.OpenWrite(pFilePath)) {
          BinaryFormatter lFormatter = new BinaryFormatter();
          lFormatter.Serialize(lStream, pObjectToSerialize);
        }
      } catch (Exception lException) {
        Logger.LogError("{0} - {1}", "Blad w Serialize", lException.Message);
      }
    }
    public object Deserialize(string pFilePath) {
      object pObject = null;
      try {
        if (!File.Exists(pFilePath))
          return null;
        BinaryFormatter lFormatter = new BinaryFormatter();
        using (FileStream lStream = File.Open(pFilePath, FileMode.Open)) {
          pObject = lFormatter.Deserialize(lStream);
        }
      } catch (Exception lException) {
        Logger.LogError("{0} - {1}", "Blad w Deserialize", lException.Message);
      }
      return pObject;
    }
    public void SerializeToXml(object pObjectToSerialize, string pFilePath) {
      try {
        XmlSerializer lXmlSerializer = new XmlSerializer(pObjectToSerialize.GetType());
        StringWriter lWriter = new StringWriter();
        lXmlSerializer.Serialize(lWriter, pObjectToSerialize);

        using (StreamWriter lStreamWriter = new StreamWriter(pFilePath)) {
          lStreamWriter.Write(lWriter.ToString());
        }
      } catch (Exception lException) {
        Logger.LogError("Blad w czasie serializacji - {0}", lException.Message);
      }
      //XmlSerializer xmlSerializer = new XmlSerializer(pObjectToSerialize.GetType());
      //string lResult;
      //using (StringWriter pStringWriter = new StringWriter()) {
      //  xmlSerializer.Serialize(pStringWriter, pObjectToSerialize);
      //  lResult = pStringWriter.ToString();
      //}
      //using (StreamWriter pStreamWriter = new StreamWriter(pFilePath)) {
      //  pStreamWriter.Write(lResult);
      //}
    }

    /// <summary>
    /// Deserializacja obiektu 
    /// </summary>
    /// <param name="pFilePath"></param>
    /// <param name="pClassType"></param>
    /// <returns></returns>
    public object DeserializeFromXml(string pFilePath, Type pClassType) {
      XmlTextReader lXmlReader = null;
      StringReader lStringReader = null;
      try {
        if (!File.Exists(pFilePath))
          return null;
        string lFileContent = null;
        using (StreamReader lReader = new StreamReader(pFilePath)) {
          lFileContent = lReader.ReadToEnd();

          lStringReader = new StringReader(lFileContent);
          XmlSerializer lXmlSerializer = new XmlSerializer(pClassType);
          lXmlReader = new XmlTextReader(lStringReader);
          Object lObject = lXmlSerializer.Deserialize(lXmlReader);
          return lObject;
        }
      } catch (Exception lException) {
        Logger.LogError("Blad w czasie deserializacji - {0}", lException.Message);
      } finally {
        if (lXmlReader != null)
          lXmlReader.Close();
        if (lStringReader != null)
          lStringReader.Close();
      }
      return null;
    }
  }

  public class SignConverter {
    private static readonly string XmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "signs.xml");
    private static readonly string XmlTagRoot = "ROOT";
    private static readonly string XmlTagItem = XmlTagRoot + "/ITEM";
    private static readonly string XmlAttributeItem = "ASCII";
    private static readonly string XmlTagUtf = "UTF";

    private static Dictionary<char, char> _charDictionary;

    public SignConverter() {
    }

    public void LoadDictionary() {
      try {
        _charDictionary = new Dictionary<char, char>();
        if (!File.Exists(XmlFilePath)) {
          Logger.LogError("Blad w {0} - {1}", "LoadDictionary", "Nie znaleziono pliku ze slownikiem znakow");
          return;
        }
        XmlDocument lXmlDocument = new XmlDocument();
        lXmlDocument.Load(XmlFilePath);
        XmlNodeList lNodeList = lXmlDocument.SelectNodes(XmlTagItem);
        if (lNodeList.Count > 0) {
          foreach (XmlNode lNode in lNodeList) {
            char lAscii = char.Parse(lNode.Attributes[XmlAttributeItem].InnerText);
            XmlNodeList lUtfList = lNode.SelectNodes(XmlTagUtf);
            foreach (XmlNode lItem in lUtfList) {
              char lUtf = char.Parse(lItem.InnerText);
              _charDictionary.Add(lUtf, lAscii);
            }
          }
        }
      } catch (Exception lException) {
        Logger.LogError("Blad w {0} - {1}", "LoadDictionary", lException.Message);
      }
      Logger.LogInfo("Zaladowano slownik znakow");
    }

    public static char ConvertSign(char pChar) {
      if (pChar < 127)
        return pChar;
      if (_charDictionary.ContainsKey(pChar))
        return _charDictionary[pChar];
      return pChar;
    }
  }
}
