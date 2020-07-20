using RDA;
using RDA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RDA2070 {

  internal class Program {

    #region Methods

    private static void Main(string[] args) {
      Engine.Init();
      //Program.ProcessingItems("ConstructionPlanItem", false);
      Program.ProcessingItems(false);
      //var z = Engine
      //   .Features.SelectMany(f=> f.Value.Descendants("Asset"))
      //   .Where(e => e.Element("Values")?.Element("Item")?.HasElements ?? false)
      //   .Select(f => f.Element("Template").Value)
      //   .Distinct();
      //foreach (var item in z) {
      //  Debug.WriteLine($"\"{item}\",");
      //}
    }
    private static void ProcessingItems(bool findSources = false) {
      var arr = new[] {
        //"ToggleHeightItem",
        //"TakeOverAction",
        //"LongDistanceRockets",
        //"UnderwaterBomb",
        //"EMP",
        "SpawnDroneItem",
        "StealthDetectionItem",
        "HijackerItem",
        "BuildingUpgradeGlobal",
        "IslandEnergyProductionUpgradeItem",
        "IslandProductionUpgradeItem",
        "VehicleUpgradeItem",
        "ResourceFillItem",
        "RepairBuilding",
        "BlackTideCleanUp",
        "StealthItem",
        "ExplosionItem",
        "DeactivateBlackSmokerItem",
        "Relic",
        "ConstructionPlanItem",
        "PetItem",
        "ArkEffectItem",
        "SeedsItem",
        "ResearchDummyItem",
        "IslandResidentBuildingsUpgradeItem",
        "QuestItem",
        "ExpeditionItem",
        "GhostStealthItem",
        "WhiteFlagItem",
        "ItemFormula",
        "TollItem",
        "SeaMineItem",
        "UseItemInArea",
        "BuildingUpgradeWorldPatron",
        "CreateLicenseItem",
        "SeaChartItem",
        "CleanNeutralEcoBuildingItem",
        "MissileDefenseItem",
        "GuidingEscortItem",
        "SearchLightItem",
        "TrashItem",
        "MilitaryBuildingUpgradeItem",
        "ShieldItem",
        "LetterOfMarqueItem",
        "HijackerStealFriendlyItem",
        "IslandPublicBuildingsUpgradeItem",
        "RepairVehicleUpgradeItem",
        "WarehouseUpgradeItem",
        "ArkUpgrade",
        "GlobalProductionBoostUpgrade",
        "BuildCostItem",
        "ProductionUpgradeItem",
        "GlobalGlobalUpgrade",
        "GlobalBuildingUpgrade",
        "GlobalUnitLimitUpgrade",
        "WorldPatronDiplomacyPreconditions",
        "WorldPatronMarketPower",
        "UnitUpgradeWorldPatron",
        "GhostDetectionItem",
        "StopProtestItem",
        "ThirdpartyTradeUpgrade",
      };

      foreach (var item in arr) {
        ProcessingItems(item, findSources);
      }
    }
    private static void ProcessingItems(String template, bool findSources = false) {
      var result = new List<Asset>();
      var document = new XDocument();
      document.Add(new XElement(template));
      foreach (var feature in Engine.Features) {
        var assets =feature.Value.XPathSelectElements($"//Asset[Template='{template}']").ToList();
        Console.WriteLine(template + "  Total: " + assets.Count);
        var count = 0;
        assets.AsParallel().ForAll(asset => {
          Console.WriteLine(asset.XPathSelectElement("Values/Standard/GUID").Value + " - " + count++);
          var item = new Asset(asset, findSources);
          item.PatchVersion = feature.Key;
          document.Root.Add(item.ToXElement());
        });
      }
    

      document.Save($@"{Engine.PathRoot}\Modified\Assets_{template}.xml");
      document.Save($@"{Engine.PathViewer}\Resources\Assets\{template}.xml");
    }

    #endregion Methods
  }
}