using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace RDA.Share {

  [Serializable]
  public class Icon {

    #region Properties

    public String Filename { get; set; }

    [XmlAttribute]
    public string FileIndex { get; set; }

    [XmlAttribute]
    public string Width { get; set; }

    [XmlAttribute]
    public string Height { get; set; }

    #endregion Properties

    #region Constructors

    public Icon() {
    }

    #endregion Constructors
  }
}