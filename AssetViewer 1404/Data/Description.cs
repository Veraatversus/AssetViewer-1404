using System;
using System.ComponentModel;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AssetViewer1404.Data {

  [Serializable]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true)]
  public class Description {

    #region Properties

    [XmlAttribute]
    public String ID { get; set; }

    public String EN { get; set; }
    public String DE { get; set; }
    public Icon Icon { get; set; }

    [XmlIgnore]
    public string CurrentLang => App.Language == Languages.German ? DE : EN;

    [XmlAttribute]
    public DescriptionFontStyle FontStyle { get; set; }

    public Description AdditionalInformation { get; set; }

    #endregion Properties

    #region Constructors

    public Description() {
    }
    public Description(XElement item) {
      this.ID = item.Attribute("ID")?.Value;
      this.EN = item.Element("EN")?.Value;
      this.DE = item.Element("DE")?.Value;
      this.Icon = item.Element("Icon")?.Value == null ? null : new Icon(item.Element("Icon"));
      var att = item.Attribute("FontStyle");
      if (att != null) {
      }
      this.FontStyle = item.Attribute("FontStyle") == null ? default : (DescriptionFontStyle)Convert.ToInt32(item.Attribute("FontStyle").Value);
      this.AdditionalInformation = item.Element("AdditionalInformation")?.Value == null ? null : new Description(item.Element("AdditionalInformation"));
    }
    public Description(string en, string de) {
      EN = en;
      DE = de;
    }

    #endregion Constructors

    #region Methods

    public Description InsertBefore(String en, String de) {
      this.EN = $"{en} {this.EN}";
      this.DE = $"{de} {this.DE}";
      return this;
    }
    public override string ToString() {
      return CurrentLang;
    }

    #endregion Methods
  }
}