using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RDA.Data {

  public class SourceWithDetailsList : List<SourceWithDetails> {

    #region Constructors

    public SourceWithDetailsList() : base() {
    }

    #endregion Constructors

    #region Methods

    public SourceWithDetailsList Copy() {
      return new SourceWithDetailsList(this.Select(l => l.Copy()));
    }

    public void AddSourceAsset(XElement element, HashSet<XElement> details = default) {
      var assetID = element.XPathSelectElement("Values/Standard/GUID").Value;
                     
      if (element.Element("Template").Value == "Quest") {
        if (element.XPathSelectElement("Asset/Values/Quest/QuestSender") == null) {
          element.XPathSelectElement("Asset/Values/Quest").Add(new XElement("QuestSender", "Quest"));
        }
        var sender = element.XPathSelectElement("Asset/Values/Quest/QuestSender").Value;
        var quest = this.Find(w => w.Source.XPathSelectElement("Asset/Values/Quest/QuestSender")?.Value == sender);
        if (quest.Source == null) {
          this.Add(new SourceWithDetails(element, details));
        }
        else {
          foreach (var item in details) {
            quest.Details.Add(item);
          }
        }
        return;
      }
      var old = this.Find(l => l.Source.XPathSelectElement("Values/Standard/GUID").Value == assetID && l.Source.Element("Template").Value == element.Element("Template").Value);
      if (old.Source != null) {
        foreach (var item in details) {
          var asset = item.Element("Asset");
          if (asset != null) {
            var itemID = item.XPathSelectElement("Values/Standard/GUID").Value;
            var itemTemplate = item.Element("Template").Value;
            if (old.Details.Any(i => i.XPathSelectElement("Values/Standard/GUID")?.Value == itemID && i.Element("Template")?.Value == itemTemplate)) {
              continue;
            }
          }
          old.Details.Add(item);
        }
      }
      else {
        this.Add(new SourceWithDetails(element, details));
      }
    }

    public void AddSourceAsset(SourceWithDetailsList input, Details details = null) {
      foreach (var item in input) {
        this.AddSourceAsset(item.Source, details?.Items ?? item.Details);
      }
    }

    #endregion Methods

    private SourceWithDetailsList(IEnumerable<SourceWithDetails> collection) : base(collection) {
    }
  }
}