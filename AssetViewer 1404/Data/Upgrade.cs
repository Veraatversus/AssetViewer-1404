using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AssetViewer1404.Data {

  public class Upgrade {

    #region Properties

    public Description Text { get; set; }
    public String Value { get; set; }
    public List<Upgrade> Additionals { get; set; }

    #endregion Properties

    #region Constructors

    public Upgrade(XElement item) {
      var textItem = item.Name == "Text" ? item : item.Element("Text");
      if (textItem != null) {
        this.Text = new Description(textItem);
      }
      if (item.Name.LocalName == "Description") {
        this.Text = new Description(item);
      }
      if (item.Element("Value") != null) {
        this.Value = item.Element("Value")?.Value;
      }
      if (item.Element("Additionals") != null) {
        this.Additionals = item.Element("Additionals").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (item.Element("Details") != null) {
        this.Additionals = item.Element("Details").Elements().Select(s => new Upgrade(s)).ToList();
      }
    }

    #endregion Constructors
  }
}