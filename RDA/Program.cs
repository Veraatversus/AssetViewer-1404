using RDA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RDA {

  internal class Program {

    #region Methods

    private static void Main(string[] args) {
      Engine.Init();
      Program.ProcessingItems("BoardersItem", true);
      Program.ProcessingItems("ConstructionItem", true);
      Program.ProcessingItems("CustomResourceItem", true);
      Program.ProcessingItems("DoctorItem", true);
      Program.ProcessingItems("DocumentItem", true);
      Program.ProcessingItems("EndlessResourceItem", true);
      Program.ProcessingItems("ExpeditionItem", true);
      Program.ProcessingItems("ExplosionItem", true);
      Program.ProcessingItems("ForceTreatyItem", true);
      Program.ProcessingItems("IslandProductionBoostItem", true);
      Program.ProcessingItems("LetterOfMarqueItem", true);
      Program.ProcessingItems("MilitaryItem", true);
      Program.ProcessingItems("PetItem", true);
      Program.ProcessingItems("PopulationItem", true);
      Program.ProcessingItems("PrestigeItem", true);
      Program.ProcessingItems("QuestItem", true);
      Program.ProcessingItems("QuestUnitItem", true);
      Program.ProcessingItems("RepairShipItem", true);
      Program.ProcessingItems("SeaChartItem", true);
      Program.ProcessingItems("SeedsItem", true);
      Program.ProcessingItems("ShipItem", true);
      Program.ProcessingItems("ShroudOfSmokeItem", true);
      Program.ProcessingItems("SpyBaseDetectionItem", true);
      Program.ProcessingItems("SpyBaseItem", true);
      Program.ProcessingItems("TollItem", true);
      Program.ProcessingItems("VenetianItem", true);
      Program.ProcessingItems("WarehouseItem", true);
      Program.ProcessingItems("WhiteFlagItem", true);
    }
    private static void ProcessingItems(String template, bool findSources = false) {
      var result = new List<Asset>();
      var document = new XDocument();
      document.Add(new XElement(template));

      var assets = Engine.Features.XPathSelectElements($"//Asset[Template='{template}']").ToList();
      Console.WriteLine(template + "  Total: " + assets.Count);
      var count = 0;
      assets.AsParallel().ForAll(asset => {
        Console.WriteLine(asset.XPathSelectElement("Values/Standard/GUID").Value + " - " + count++);
        var item = new Asset(asset, findSources);
        document.Root.Add(item.ToXElement());
      });

      document.Save($@"{Engine.PathRoot}\Modified\Assets_{template}.xml");
      document.Save($@"{Engine.PathViewer}\Resources\Assets\{template}.xml");
    }

    #endregion Methods
  }
}