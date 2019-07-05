using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AssetViewer1404 {

  public static class XmlExtensions {

    #region Methods

    public static XElement ToXElement<T>(this T obj, Func<T, XElement, XElement> modifyFunktion = null) {
      using (var memoryStream = new MemoryStream()) {
        using (TextWriter streamWriter = new StreamWriter(memoryStream)) {
          var ns = new XmlSerializerNamespaces();
          ns.Add("", "");
          var xmlSerializer = new XmlSerializer(typeof(T));
          xmlSerializer.Serialize(streamWriter, obj, ns);
          var xElement = XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
          modifyFunktion?.Invoke(obj, xElement);
          return xElement;
        }
      }
    }

    public static T FromXElement<T>(this XElement xElement, Func<T, XElement, T> modifyFunktion = null) {
      var xmlSerializer = new XmlSerializer(typeof(T));
      var obj = (T)xmlSerializer.Deserialize(xElement.CreateReader());
      modifyFunktion?.Invoke(obj, xElement);
      return obj;
    }

    public static Stream ToStream(this string @this) {
      var stream = new MemoryStream();
      var writer = new StreamWriter(stream);
      writer.Write(@this);
      writer.Flush();
      stream.Position = 0;
      return stream;
    }

    public static T DeserializeObject<T>(this string xml)
         where T : new() {
      if (string.IsNullOrEmpty(xml)) {
        return new T();
      }
      try {
        using (var stringReader = new StringReader(xml)) {
          var serializer = new XmlSerializer(typeof(T));
          return (T)serializer.Deserialize(stringReader);
        }
      }
      catch (Exception ex) {
        return new T();
      }
    }
  }

  #endregion Methods
}