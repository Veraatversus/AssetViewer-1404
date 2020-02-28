﻿using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RDA {

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

    #endregion Methods
  }
}