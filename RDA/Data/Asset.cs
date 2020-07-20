using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace RDA.Data {

  [XmlRoot(Namespace = "")]
  public class Asset {

    #region Properties

    public static ConcurrentDictionary<string, SourceWithDetailsList> SavedSources { get; set; } = new ConcurrentDictionary<string, SourceWithDetailsList>();

    [XmlAttribute]
    public string ID { get; set; }

    [XmlAttribute]
    public string Name { get; set; }

    public Description Text { get; set; }
    public string ItemQuality { get; set; }
    public Description ItemType { get; set; }
    public string ItemCategory { get; set; }
    public string ItemTarget { get; set; }
    public Description HonourPrice { get; set; }
    public string Cooldown { get; set; }
    public string EffectDuration { get; set; }
    public string UtilizationCount { get; set; }
    public List<Description> AffectTargets { get; set; }
    public List<Description> TargetCategories { get; set; } = new List<Description>();

    public UpgradeList CostUpgrades { get; set; }
    public UpgradeList HitpointUpgrade { get; set; }
    public UpgradeList ShipCombatUpgrade { get; set; }
    public UpgradeList ShipRepairUpgrade { get; set; }
    public UpgradeList ShipUpgrade { get; set; }
    public UpgradeList TransportUpgrade { get; set; }
    public UpgradeList UnitUpgrade { get; set; }
    public UpgradeList Upgrade { get; set; }
    public UpgradeList BuildCostUpgrade { get; set; }
    public UpgradeList FactoryUpgrade { get; set; }
    public UpgradeList ProductionUpgrade { get; set; }
    public UpgradeList PublicBuildingUpgrade { get; set; }
    public UpgradeList WarehouseUpgrade { get; set; }
    public UpgradeList GlobalUpgrade { get; set; }
    public UpgradeList PrestigeItem { get; set; }
    public UpgradeList Document { get; set; }
    public UpgradeList MilitaryUnitUpgrade { get; set; }
    public UpgradeList TollBalancing { get; set; }
    public UpgradeList Seed { get; set; }
    public bool IsQuestItem { get; set; }
    public bool IsPassiveItem { get; set; }
    public List<TempSource> Sources { get; set; }
    public UpgradeList CustomResource { get; private set; }
    public UpgradeList DoctorUpgrade { get; private set; }
    public UpgradeList SpyBaseDetectionBalancing { get; private set; }
    public UpgradeList VenetianUpgrade { get; private set; }
    public UpgradeList EndlessResource { get; private set; }

    #endregion Properties

    #region Constructors

    public Asset() {
    }
    public Asset(XElement asset, Boolean findSources) {
      foreach (var element in asset.Element("Values").Elements()) {
        switch (element.Name.LocalName) {
          case "GameAction":
            //Ignore
            break;

          case "Standard":
            this.ProcessElement_Standard(element);
            break;

          case "Item":
            this.ProcessElement_Item(element);
            break;

          case "CostUpgrade":
            this.ProcessElement_CostUpgrade(element);
            break;

          case "HitpointUpgrade":
            this.ProcessElement_HitpointUpgrade(element);
            break;

          case "ShipCombatUpgrade":
            this.ProcessElement_ShipCombatUpgrade(element);
            break;

          case "ShipRepairUpgrade":
            this.ProcessElement_ShipRepairUpgrade(element);
            break;

          case "ShipUpgrade":
            this.ProcessElement_ShipUpgrade(element);
            break;

          case "TransportUpgrade":
            this.ProcessElement_TransportUpgrade(element);
            break;

          case "UnitUpgrade":
            this.ProcessElement_UnitUpgrade(element);
            break;

          case "Upgrade":
            this.ProcessElement_Upgrade(element);
            break;

          case "BuildCostUpgrade":
            this.ProcessElement_BuildCostUpgrade(element);
            break;

          case "FactoryUpgrade":
            this.ProcessElement_FactoryUpgrade(element);
            break;

          case "ProductionUpgrade":
            this.ProcessElement_ProductionUpgrade(element);
            break;

          case "PublicBuildingUpgrade":
            this.ProcessElement_PublicBuildingUpgrade(element);
            break;

          case "WarehouseUpgrade":
            this.ProcessElement_WarehouseUpgrade(element);
            break;

          case "GlobalUpgrade":
            this.ProcessElement_GlobalUpgrade(element);
            break;

          case "Seed":
            this.ProcessElement_Seed(element);
            break;

          case "PrestigeItem":
            this.ProcessElement_PrestigeItem(element);
            break;

          case "Document":
            this.ProcessElement_Document(element);
            break;

          case "MilitaryUnitUpgrade":
            this.ProcessElement_MilitaryUnitUpgrade(element);
            break;

          case "TollBalancing":
            this.ProcessElement_TollBalancing(element);
            break;        
          case "CustomResource":
            this.ProcessElement_CustomResource(element);
            break;         
          case "DoctorUpgrade":
            this.ProcessElement_DoctorUpgrade(element);
            break;   
          case "VenetianUpgrade":
            this.ProcessElement_VenetianUpgrade(element);
            break;         
          case "EndlessResource":
            this.ProcessElement_EndlessResource(element);
            break;
          case "SpyBaseDetectionBalancing":
            this.ProcessElement_SpyBaseDetectionBalancing(element);
            break;
          //Todo
          case "ExplosionBalancing":
          case "ExpeditionBalancing":
          case "BoardersBalancing":
          case "ShroudOfSmokeBalancing":

          case "ForceTreatyBalancing":
          case "Pet":

            break;

          default:
            Debug.WriteLine(element.Name.LocalName);
            break;
        }
      }
      if (findSources) {
        var sources = this.FindSources(this.ID).ToArray();
        this.Sources = sources.Select(s => new TempSource(s)).ToList();
      }
    }

    #endregion Constructors

    #region Methods

    private SourceWithDetailsList FindSources(string id, Details mainDetails = default, SourceWithDetailsList inResult = default) {
      mainDetails = (mainDetails == default) ? new Details() : mainDetails;
      mainDetails.PreviousIDs.Add(id);
      var mainResult = inResult ?? new SourceWithDetailsList();
      var resultstoadd = new List<SourceWithDetailsList>();
      var links = Engine.AllXmls.SelectMany(x => x.XPathSelectElements($"//*[text()={id} and not(self::GUID)]")).ToArray();
      if (links.Length > 0) {
        for (var i = 0; i < links.Length; i++) {
          var element = links[i];

          var isShipDrop = element.Name.LocalName == "ShipLoadPool";

          while (element.Name.LocalName != "Asset" || !element.HasElements) {
            element = element.Parent;
          }
          var Details = new Details(mainDetails);
          var result = mainResult.Copy();
          var key = element.XPathSelectElement("Values/Standard/GUID").Value;

          if (isShipDrop) {
            if (element.XPathSelectElement("Values/ShipSpawner/ShipLoadPool")?.Value == id) {
              result.AddSourceAsset(element.GetProxyElement("ShipDrop"), new HashSet<XElement> { element.GetProxyElement("ShipDrop") });
              resultstoadd.Add(result);
              continue;
            }
          }

          if (mainDetails.PreviousIDs.Contains(key)) {
            continue;
          }
          switch (element.Element("Template").Value) {
            case "MobileArmy":
            case "MilitaryCamp":
            case "MilitaryCastle":
            case "WarShip":
            case "TradingShip":
            case "BoardersItem":    //doesnt know
            case "UpgradeBuilding":   
              //Ignore
              break;

            case "WinConditionQuest":
            case "Treibgut_Quest":
            case "Schiffbruechigen_Quest":
            case "Flotten_Quest":
            case "Eskortieren_Quest":
            case "Havarie_Quest":
            case "Schiff abfangen_Quest":
            case "Wimmelbild Mönch_Quest":
            case "Wimmelbild Spion_Quest":
            case "Lieferungs_Quest":
            case "Kartographieren_Quest":
            case "Robinson_Crusoe_Quest":
            case "Schatzsuche_Quest":
            case "HilferufQuest":
            case "TradeWaresQuest":
            case "Schiff abfangen mit Eskorte_Quest":
            case "Wimmelbild Orient_Ritter_Quest":
            case "Blockade_Quest":
            case "Gold_erreichen_Quest":
            case "Wimmelbild Ritter_Quest":
            case "Wimmelbild Hexe_Quest":
            case "Wimmelbild Narr_Quest":
            case "GetResidentLevelQuest":
            case "Wimmelbild Alchemist_Quest":
            case "Wimmelbild Bauchtänzerin_Quest":
            case "Ruhm_erreichen_Quest":
            case "Wimmelbild Korsar_Quest":
            case "Wimmelbild Assassine_Quest":
            case "Wimmelbild Nonne_Quest":
            case "UseItemQuest":
            case "NeedWaresQuest":
            case "CreateBuildingQuest":
            case "EconomyHelperQuest":
            case "GetHonourPointsQuest":
            case "ColonizeIslandQuest":
            case "NeededMoneyQuest":
            case "StreetConnectionQuest":
            case "FarmConnectionQuest":
            case "Wimmelbild Osterei_Quest":
            case "SiegbedingungsQuest":
            case "PrestigeItemsQuest":
            case "ShipRewardQuest":
            case "Wimmelbild Gaukler_Quest":
            case "FunctionQuest":
            case "KorsarenJagd":
            case "HandelsschiffJagd":
            case "KriegsschiffJagd":
            case "TradingRaceQuest":
            case "BoardShip":
            case "TradingRaceQuestOpponent":
            case "BoardShipWithEscort":
            case "TradingRaceQuestDolphin":
              result.AddSourceAsset(element.GetProxyElement("Quest"), new HashSet<XElement> { element.GetProxyElement("Quest") });
              break;

            case "EasyCSP":
            case "HardCSP":
            case "MediumCSP":
              if (element.XPathSelectElement("Values/ShipFight/ShipDropPool")?.Value == id) {
                result.AddSourceAsset(element.GetProxyElement("ShipDrop"), new HashSet<XElement> { element.GetProxyElement("ShipDrop") });
              }
              else {
              }
              break;

            case "ExpeditionItem":
              if (element.XPathSelectElement("Values/ExpeditionBalancing/ExpeditionReward")?.Value == id ||
                  element.XPathSelectElement("Values/ExpeditionBalancing/ExpeditionItemReward")?.Value == id) {
                result.AddSourceAsset(element.GetProxyElement("Expedition"), new HashSet<XElement> { element.GetProxyElement("Expedition") });
              }
              else {
              }
              break;

            case "Corsair":
            case "Harbour":
              result.AddSourceAsset(element.GetProxyElement("Harbor"), new HashSet<XElement> { element });
              break;

            case "Reward":
            case "RewardPool":
              if (SavedSources.ContainsKey(key)) {
                result.AddSourceAsset(SavedSources[key].Copy());
                break;
              }
              FindSources(key, Details, result);
              if (!SavedSources.ContainsKey(key)) {
                SavedSources.TryAdd(key, result.Copy());
              }
              break;

            default:
              Debug.WriteLine(element.Element("Template")?.Value);
              break;
          }
          resultstoadd.Add(result);
        }
      }
      foreach (var item in resultstoadd) {
        mainResult.AddSourceAsset(item);
      }
      return mainResult;
    }

    private void ProcessElement_TollBalancing(XElement element) {
      if (element.HasElements) {
        this.TollBalancing = new UpgradeList();
        this.TollBalancing.Add(new Upgrade(element));
      }
    }         
    private void ProcessElement_CustomResource(XElement element) {
      if (element.HasElements) {
          this.CustomResource = new UpgradeList();
          this.CustomResource.Add(new Upgrade(element));
      }
    }

    private void ProcessElement_MilitaryUnitUpgrade(XElement element) {
      //Todo
      //if (element.HasElements) {
      //  this.MilitaryUnitUpgrade = new UpgradeList();
      //  foreach (var item in element.Elements()) {
      //    this.MilitaryUnitUpgrade.Add(new Upgrade(item));
      //  }
      //}
    }

    private void ProcessElement_Document(XElement element) {
      if (element.HasElements) {
        this.Document = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.Document.Add(new Upgrade(item));
        }
      }
    }           
    private void ProcessElement_DoctorUpgrade(XElement element) {
      if (element.HasElements) {
        this.DoctorUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.DoctorUpgrade.Add(new Upgrade(item));
        }
      }
    }                 
    private void ProcessElement_VenetianUpgrade(XElement element) {
      if (element.HasElements) {
        this.VenetianUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.VenetianUpgrade.Add(new Upgrade(item));
        }
      }
    }         
    private void ProcessElement_EndlessResource(XElement element) {
        this.EndlessResource = new UpgradeList();
        this.EndlessResource.Add(new Upgrade(element));
    }       
    private void ProcessElement_SpyBaseDetectionBalancing(XElement element) {
      if (element.HasElements) {
        this.SpyBaseDetectionBalancing = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.SpyBaseDetectionBalancing.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_PrestigeItem(XElement element) {
      if (element.HasElements) {
        this.PrestigeItem = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.PrestigeItem.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_Seed(XElement element) {
      if (element.HasElements) {
        this.Seed = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.Seed.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_GlobalUpgrade(XElement element) {
      if (element.HasElements) {
        this.GlobalUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.GlobalUpgrade.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_WarehouseUpgrade(XElement element) {
      if (element.HasElements) {
        this.WarehouseUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.WarehouseUpgrade.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_PublicBuildingUpgrade(XElement element) {
      if (element.HasElements) {
        this.PublicBuildingUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.PublicBuildingUpgrade.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_ProductionUpgrade(XElement element) {
      if (element.HasElements) {
        this.ProductionUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          if (item.Name.LocalName != "WorkerCountType") {
          this.ProductionUpgrade.Add(new Upgrade(item));
          }
        }
      }
    }

    private void ProcessElement_FactoryUpgrade(XElement element) {
      if (element.HasElements) {
        throw new NotImplementedException();
      }
    }

    private void ProcessElement_BuildCostUpgrade(XElement element) {
      if (element.HasElements) {
        this.BuildCostUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.BuildCostUpgrade.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_Upgrade(XElement element) {
      if (element.HasElements) {
        this.Upgrade = new UpgradeList();
        this.AffectTargets = new List<Description>();
        if (element.Element("TargetGUIDs")?.HasElements ?? false) {
          foreach (var item in element.Descendants("UpgradeGUID")) {
            this.AffectTargets.Add(new Description(item.Value));
          }
        }
        if (element.Element("TargetGUID")?.Value is string guid) {
          this.AffectTargets.Add(new Description(guid));
        }
        if (element.Element("TargetCategories")?.Value != null) {
          foreach (var item in element.Element("TargetCategories").Value.Split(';')) {
            this.TargetCategories.Add(new Description(item));
          }
        }
      }
    }

    private void ProcessElement_UnitUpgrade(XElement element) {
      if (element.HasElements) {
        this.UnitUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.UnitUpgrade.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_TransportUpgrade(XElement element) {
      if (element.HasElements) {
        this.TransportUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.TransportUpgrade.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_ShipUpgrade(XElement element) {
      if (element.HasElements) {
        this.ShipUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.ShipUpgrade.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_ShipRepairUpgrade(XElement element) {
      if (element.HasElements) {
        this.ShipRepairUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.ShipRepairUpgrade.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_ShipCombatUpgrade(XElement element) {
      if (element.HasElements) {
        this.ShipCombatUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.ShipCombatUpgrade.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_HitpointUpgrade(XElement element) {
      if (element.HasElements) {
        this.HitpointUpgrade = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.HitpointUpgrade.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_CostUpgrade(XElement element) {
      if (element.HasElements) {
        this.CostUpgrades = new UpgradeList();
        foreach (var item in element.Elements()) {
          this.CostUpgrades.Add(new Upgrade(item));
        }
      }
    }

    private void ProcessElement_Standard(XElement element) {
      this.ID = element.Element("GUID").Value;
      this.Name = element.Element("Name").Value;
      this.Text = new Description(element.Element("GUID").Value);
      //this.Info = element.Element("InfoDescription") == null ? null : new Description(element.Element("InfoDescription").Value);
    }
    private void ProcessElement_Item(XElement element) {
      this.ItemQuality = element.Element("ItemQuality")?.Value;
      if (element.Element("ItemType")?.Value is string itemtype) {
        if (Engine.NamesToId.ContainsKey(itemtype)) {
          this.ItemType = new Description(itemtype);
        }
        else {
          this.ItemType = new Description(itemtype, itemtype);
        }

        var r = new Regex("(\\[)(GUIDNAME)(\\s+)(\\d+)(\\])", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        var matches = r.Matches(this.ItemType.DE).Cast<Match>();
        if (matches.Any()) {
          foreach (var match in matches) {
            this.ItemType.Replace(match.ToString(), new Description(match.Groups[4].Value));
          }
        }
        this.IsQuestItem = element.Element("IsQuestItem")?.Value == "1";
        this.IsPassiveItem = element.Element("Passive")?.Value == "1";
      }

      this.ItemCategory = element.Element("ItemCategory")?.Value;
      this.ItemTarget = element.Element("ItemTarget")?.Value;
      if (element.Element("ItemHonourPrice")?.Value is string str) {
        this.HonourPrice = new Description("140009").InsertBefore(str + " ", str + " ");
      }

      this.Cooldown = element.Element("Cooldown")?.Value;
      this.EffectDuration = element.Element("EffectDuration")?.Value;
      this.UtilizationCount = element.Element("UtilizationCount")?.Value;
    }

    #endregion Methods
  }
}