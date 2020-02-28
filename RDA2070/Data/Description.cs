using RDA.Share;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace RDA.Data {

  [Serializable]
  public class Description : RDA.Share.Description {

    #region Properties

    #endregion Properties

    #region Constructors

    public Description(String en, String de, Icon icon = null, Description AdditionalInformation = null, DescriptionFontStyle fontStyle = default) : base(en, de, icon, AdditionalInformation, fontStyle) {

    }
    public Description() {
    }
    public Description(String textId, DescriptionFontStyle fontStyle = default) {
      var id = textId;
      if (Engine.NamesToId.ContainsKey(id)) {
        id = Engine.NamesToId[id];
      }
      if (Engine.Text.ContainsKey(id)) {
        this.ID = id;
        this.DE = Engine.Text[id][Languages.German].text;
        if (Engine.Text[id].TryGetValue(Languages.English, out var text)) {
          this.EN = text.text;
        }
        else {
          this.EN = Engine.Text[id][Languages.German].text;
        }
        if (Engine.Text[id][Languages.German].subtext != null) {
          this.AdditionalInformation = new Description();
          this.AdditionalInformation.DE = Engine.Text[id][Languages.German].subtext.FirstOrDefault()?.Replace("[CR]", "");

          this.AdditionalInformation.EN = Engine.Text[id].TryGetValue(Languages.English, out var subtext) ? subtext.subtext.FirstOrDefault()?.Replace("[CR]", "") : Engine.Text[id][Languages.German].subtext.FirstOrDefault()?.Replace("[CR]", "");
        }
      }

      if (Engine.Icons.ContainsKey(this.ID)) {
        this.Icon = new Icon(this.ID);
      }
      this.FontStyleString = fontStyle;
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