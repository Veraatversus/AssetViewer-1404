using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace RDA.Data {

  [Serializable]
  public class Upgrade {

    #region Properties

    public Description Text { get; set; }
    public String Value { get; set; }
    public List<Upgrade> Additionals { get; set; }

    #endregion Properties

    #region Constructors

    public Upgrade() {
    }
    public Upgrade(XElement element) {
      var isPercent = element.Element("Percental") == null ? false : element.Element("Percental").Value == "1";
      var value = element.Element("Value") == null ? null : (Int32?)Int32.Parse(element.Element("Value").Value);
      var factor = 1;

      if (Engine.NamesToId.ContainsKey(element.Name.LocalName)) {
        this.Text = new Description(element.Name.LocalName);
      }
      else if (Engine.NamesToId.ContainsKey(element.Parent.Parent.Element("Item").Element("ItemCategory").Value + element.Name.LocalName)) {
        this.Text = new Description(Engine.NamesToId[element.Parent.Parent.Element("Item").Element("ItemCategory").Value + element.Name.LocalName]);
      }
      switch (element.Name.LocalName) {
        case "MoneyCost":
        case "ActiveCost":
        case "InactiveCost":
        case "HonorCost":
        case "MaxHitpoints":
        case "DamagePerSecond":
        case "AttackSpeed":
        case "HealingPointsPerMinute":
        case "SelfHealingPointsPerMinute":
        case "VisionRadius":
        case "LoadDamageCap":
        case "SlotCapacity":
        case "SlotCount":
        case "WalkingSpeed":
        case "AttackRange":
        case "NoProduct":
        case "Wood":
        case "Tools":
        case "Stone":
        case "Glass":
        case "Mosaic":
        case "Gold":
        case "Ropes":
        case "Weapons":
        case "WarMaschines":
        case "Cannons":
        case "Nobleman":
        case "Patrician":
        case "Citizen":
        case "Ambassador":
        case "WorkerCount":
        case "ProductionTime":
        case "ProductionCapacity":
        case "ProductionCount":
        case "IsNoriaUpgrade":
        case "BuildingDemandEfficiency":
        case "InfluenceRadius":
        case "Capacity":
        case "DemandAmount":
        case "TaxUpgradePercent":
        case "AmbassadorUpgradeRightBonus":
        case "BeggarUpgradeRightBonus":
        case "DamageCloseCombatMax":
        case "DamageCloseCombatMin":
        case "DamageRangeCombatMax":
        case "DamageRangeCombatMin":
        case "AttackDelayCloseCombat":
        case "AttackDelayRangeCombat":
        case "DoctorHealingTime":
        case "EndlessResource":
        case "SharePriceForeignUpgrade":
        case "SharePriceOwnerUpgrade":
          break;

        case "GeneratedFertilityClime":
          if (element.Value == "North") {
            this.Text = new Description("137593");
          }
          else if (element.Value == "South") {
            this.Text = new Description("137594");
          }
          else {
          }
          break;

        case "GeneratedFertility":
          this.Text.Replace("[FERTILITYNAME]", new Description(element.Value));
          break;

        case "PrestigePoints":
        case "MinTradeAmountForTollGeneration":
          value = Int32.Parse(element.Value);
          break;

        case "DocumentText":
          this.Text = new Description("Text:", "Text:");
          this.Text.AdditionalInformation = new Description(element.Value);
          break;

        case "DamageEfficiency":
        case "ResidenceUpgradeAmountMaxPercent":
        case "ProductCost":
          Additionals = new List<Upgrade>();
          foreach (var item in element.Elements()) {
            if (item.Name.LocalName != "NoProduct") {
              Additionals.Add(new Upgrade(item));
            }
          }
          break;

        case "TollMoney":
          this.Text = new Description("TollMoney");
          value = Int32.Parse(element.Value);
          break;    
          
        case "DiscoverAllChance":
          this.Text = new Description("DiscoverAllChance");
          value = Int32.Parse(element.Value);
          break;

        case "TollProducts":
          this.Additionals = new List<Upgrade>();
          foreach (var item in element.Elements()) {
            if (item.Name.LocalName != "NoProduct") {
              this.Additionals.Add(new Upgrade { Text = new Description(item.Element("Product").Value), Value = item.Element("Amount").Value });
            }
          }
          break;

        case "CustomResource":
          this.Additionals = new List<Upgrade>();
          foreach (var ele in element.Elements()) {
            foreach (var item in ele.Value.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)) {
              this.Additionals.Add(new Upgrade { Text = new Description(item) });

            }
          }
          break;

        case "TollBalancing":
          this.Additionals = new List<Upgrade>();

          if (element.Element("TollMoney") != null) {
            this.Additionals.Add(new Upgrade(element.Element("TollMoney")));
          }
          if (element.Element("TollProducts") != null) {
            foreach (var item in element.Element("TollProducts").Elements()) {
              this.Additionals.Add(new Upgrade { Text = new Description(item.Element("Product").Value), Value = item.Element("Amount").Value });
            }
          }
          if (element.Element("MinTradeAmountForTollGeneration")?.Value is String str) {
            this.Text.AdditionalInformation = new Description("MinTradeAmountForTollGeneration");
          }
          break;

        default:
          throw new NotImplementedException(element.Name.LocalName);
      }
      if (value == null) {
        this.Value = String.Empty;
      }
      else {
        if (isPercent) {
          this.Value = value > 0 ? $"+{value * factor}%" : $"{value * factor}%";
        }
        else {
          this.Value = value > 0 ? $"+{value * factor}" : $"{value * factor}";
        }
      }
    }

    #endregion Constructors
  }
}