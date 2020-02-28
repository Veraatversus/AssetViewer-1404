using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.XPath;

namespace RDA.Data {

  public class TempSource {

    #region Properties

    public String ID { get; set; }
    public Description Text { get; set; }
    public List<Description> Details { get; set; } = new List<Description>();

    #endregion Properties

    #region Constructors

    public TempSource(SourceWithDetails element) {
      var Source = element.Source;
      this.ID = Source.XPathSelectElement("Values/Standard/GUID").Value;
      //this.Name = Source.XPathSelectElement("Values/Standard/Name").Value;
      switch (Source.Element("Template").Value) {
        case "Expedition":
          this.Text = new Description(Source.XPathSelectElement("Values/Standard/GUID").Value);
          this.Text.EN = $"Expedition - {this.Text.EN}";
          this.Text.DE = $"Expedition - {this.Text.DE}";
          this.Details = element.Details.Select(d => new Description(d.XPathSelectElement("Values/Standard/GUID").Value)).ToList();
          break;

        case "ShipDrop":
          this.Text = new Description(element.Source.XPathSelectElement("Values/Standard/GUID").Value);
          this.Text.EN = $"Ship Drop - {this.Text.EN}";
          this.Text.DE = $"Schiff Drop - {this.Text.DE}";
          this.Details = element.Details.Select(d => new Description(d.XPathSelectElement("Values/Standard/GUID").Value)).ToList();
          break;

        case "Harbor":
          this.Text = new Description(Source.XPathSelectElement("Values/Standard/GUID").Value);
          this.Text.EN = $"Harbour - {this.Text.EN}";
          this.Text.DE = $"Hafen - {this.Text.DE}";
          this.Details = element.Details.Select(d => new Description(d.XPathSelectElement("Values/Standard/GUID").Value)).ToList();
          break;

        case "Quest":
          if (Source.XPathSelectElement("Asset/Values/Quest/QuestSender")?.Value == "Quest") {
            this.Text = new Description("Quest", "Quest");
            //this.Text.EN = $"Quest - {this.Text.EN}";
            //this.Text.DE = $"Quest - {this.Text.DE}";
          }
          else {
            this.Text = new Description(Source.XPathSelectElement("Asset/Values/Quest/QuestSender").Value);
            this.Text.EN = $"Quest - {this.Text.EN}";
            this.Text.DE = $"Quest - {this.Text.DE}";
          }
          foreach (var item in element.Details) {
            var desc = new Description(item.XPathSelectElement("Values/Standard/GUID").Value);
            this.Details.Add(desc);
          }
          break;

        default:
          Debug.WriteLine(Source.Element("Template").Value);
          break;
      }
    }
    public TempSource() {
    }

    #endregion Constructors
  }
}