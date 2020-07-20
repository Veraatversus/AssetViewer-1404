using System;
using System.Linq;
using System.Xml.Serialization;

namespace RDA.Share {
  [Serializable]
  public class DescriptionBase : IEquatable<DescriptionBase> {

    #region Properties

    [XmlAttribute]
    public String ID { get; set; }

    public String EN { get; set; }
    public String DE { get; set; }
    public IconBase Icon { get; set; }

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

    public DescriptionBase AdditionalInformation { get; set; }

    #endregion Properties

    #region Constructors

    public DescriptionBase(String en, String de, IconBase icon = null, DescriptionBase AdditionalInformation = null, DescriptionFontStyle fontStyle = default) {
      this.ID = String.Empty;
      this.EN = en;
      this.DE = de;
      this.Icon = icon;
      this.AdditionalInformation = AdditionalInformation;
      this.FontStyleString = fontStyle;
    }
    public DescriptionBase() {
    }

    #endregion Constructors

    #region Methods

    public void Replace(string oldValue, DescriptionBase newValue) {
      this.DE = this.DE.Replace(oldValue, newValue.DE);
      this.EN = this.EN.Replace(oldValue, newValue.EN);
    }
    public void Append(DescriptionBase newValue) {
      this.DE = this.DE + newValue.DE;
      this.EN = this.EN + newValue.EN;
    }
    public DescriptionBase InsertBefore(String en, String de) {
      this.EN = $"{en} {this.EN}";
      this.DE = $"{de} {this.DE}";
      return this;
    }
    public override String ToString() {
      return this.EN;
    }

    public bool Equals(DescriptionBase other) {
      return ID == other.ID && EN == other.EN && DE == other.DE && Icon == other.Icon;
    }

    #endregion Methods
  }
}