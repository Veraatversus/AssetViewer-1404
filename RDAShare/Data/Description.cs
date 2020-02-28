using System;
using System.Linq;
using System.Xml.Serialization;

namespace RDA.Share {

  [Serializable]
  public class Description : IEquatable<Description> {

    #region Properties

    [XmlAttribute]
    public String ID { get; set; }

    public String EN { get; set; }
    public String DE { get; set; }
    public Icon Icon { get; set; }

    [XmlIgnore]
    public DescriptionFontStyle FontStyleString { get; set; }

    [XmlAttribute]
    public int FontStyle {
      get {
        return (int)FontStyleString;
      }
      set {
        FontStyleString = (DescriptionFontStyle)value;
      }
    }

    public Description AdditionalInformation { get; set; }

    #endregion Properties

    #region Constructors

    public Description(String en, String de, Icon icon = null, Description AdditionalInformation = null, DescriptionFontStyle fontStyle = default) {
      this.ID = String.Empty;
      this.EN = en;
      this.DE = de;
      this.Icon = icon;
      this.AdditionalInformation = AdditionalInformation;
      this.FontStyleString = fontStyle;
    }
    public Description() {
    }

    #endregion Constructors

    #region Methods

    public void Replace(string oldValue, Description newValue) {
      this.DE = this.DE.Replace(oldValue, newValue.DE);
      this.EN = this.EN.Replace(oldValue, newValue.EN);
    }
    public void Append(Description newValue) {
      this.DE = this.DE + newValue.DE;
      this.EN = this.EN + newValue.EN;
    }
    public Description InsertBefore(String en, String de) {
      this.EN = $"{en} {this.EN}";
      this.DE = $"{de} {this.DE}";
      return this;
    }
    public override String ToString() {
      return this.EN;
    }

    public bool Equals(Description other) {
      return ID == other.ID && EN == other.EN && DE == other.DE && Icon == other.Icon;
    }

    #endregion Methods
  }
}