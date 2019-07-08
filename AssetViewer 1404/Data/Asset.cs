using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AssetViewer1404.Data {

  public class Asset {

    #region Properties

    public string ID { get; set; }
    public string Name { get; set; }
    public Description Text { get; set; }
    public string ItemQuality { get; set; }
    public Description ItemType { get; set; }
    public string ItemCategory { get; set; }
    public string ItemTarget { get; set; }
    public Description HonourPrice { get; set; }
    public string Cooldown { get; set; }
    public string EffectDuration { get; private set; }
    public string UtilizationCount { get; private set; }
    public List<Description> AffectTargets { get; set; } = new List<Description>();
    public List<Description> TargetCategories { get; set; } = new List<Description>();
    public List<Upgrade> CostUpgrades { get; set; }
    public List<Upgrade> HitpointUpgrade { get; set; }
    public List<Upgrade> ShipCombatUpgrade { get; set; }
    public List<Upgrade> ShipRepairUpgrade { get; set; }
    public List<Upgrade> ShipUpgrade { get; set; }
    public List<Upgrade> TransportUpgrade { get; set; }
    public List<Upgrade> UnitUpgrade { get; set; }
    public List<Upgrade> Upgrade { get; set; }
    public List<Upgrade> BuildCostUpgrade { get; set; }
    public List<Upgrade> FactoryUpgrade { get; set; }
    public List<Upgrade> ProductionUpgrade { get; set; }
    public List<Upgrade> PublicBuildingUpgrade { get; set; }
    public List<Upgrade> WarehouseUpgrade { get; set; }
    public List<Upgrade> GlobalUpgrade { get; set; }
    public List<Upgrade> PrestigeItem { get; set; }
    public List<Upgrade> Document { get; set; }
    public List<Upgrade> MilitaryUnitUpgrade { get; set; }
    public List<Upgrade> TollBalancing { get; set; }
    public List<Upgrade> Seed { get; set; }
    public List<Upgrade> Sources { get; set; } = new List<Upgrade>();
    public Description AffectTargetsInfo => AffectTargets.Join(", ").InsertBefore("Affects", "Beeinflusst");

    public IEnumerable<Upgrade> AllUpgrades => typeof(Asset)
.GetProperties()
.Where(p => p.PropertyType == typeof(List<Upgrade>) && p.Name != nameof(Sources))
.SelectMany(l => (l.GetValue(this) as List<Upgrade>) ?? Enumerable.Empty<Upgrade>());

    #endregion Properties

    #region Constructors

    public Asset(XElement asset) {
      this.ID = asset.Attribute("ID").Value;
      this.Name = asset.Attribute("Name").Value;
      this.Text = new Description(asset.Element("Text"));
      this.ItemQuality = asset.Element("ItemQuality")?.Value;
      this.ItemType = asset.Element("ItemType") == null ? null : new Description(asset.Element("ItemType"));
      this.ItemCategory = asset.Element("ItemCategory")?.Value;
      this.ItemTarget = asset.Element("ItemTarget")?.Value;
      this.HonourPrice = asset.Element("HonourPrice") == null ? null : new Description(asset.Element("HonourPrice"));
      this.Cooldown = asset.Element("Cooldown")?.Value;
      this.EffectDuration = asset.Element("EffectDuration")?.Value;
      this.UtilizationCount = asset.Element("UtilizationCount")?.Value;
      if (asset.Element("AffectTargets") != null && asset.Element("AffectTargets").HasElements) {
        this.AffectTargets = asset.Element("AffectTargets").Elements().Select(s => new Description(s)).ToList();
      }
      if (asset.Element("TargetCategories")?.HasElements ?? false) {
        this.TargetCategories = asset.Element("TargetCategories").Elements().Select(s => new Description(s)).ToList();
      }
      if (asset.Element("CostUpgrades")?.HasElements ?? false) {
        this.CostUpgrades = asset.Element("CostUpgrades").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("HitpointUpgrade")?.HasElements ?? false) {
        this.HitpointUpgrade = asset.Element("HitpointUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("ShipCombatUpgrade")?.HasElements ?? false) {
        this.ShipCombatUpgrade = asset.Element("ShipCombatUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("ShipRepairUpgrade")?.HasElements ?? false) {
        this.ShipRepairUpgrade = asset.Element("ShipRepairUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("ShipUpgrade")?.HasElements ?? false) {
        this.ShipUpgrade = asset.Element("ShipUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("TransportUpgrade")?.HasElements ?? false) {
        this.TransportUpgrade = asset.Element("TransportUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("UnitUpgrade")?.HasElements ?? false) {
        this.UnitUpgrade = asset.Element("UnitUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("Upgrade")?.HasElements ?? false) {
        this.Upgrade = asset.Element("Upgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("BuildCostUpgrade")?.HasElements ?? false) {
        this.BuildCostUpgrade = asset.Element("BuildCostUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("FactoryUpgrade")?.HasElements ?? false) {
        this.FactoryUpgrade = asset.Element("FactoryUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("ProductionUpgrade")?.HasElements ?? false) {
        this.ProductionUpgrade = asset.Element("ProductionUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("PublicBuildingUpgrade")?.HasElements ?? false) {
        this.PublicBuildingUpgrade = asset.Element("PublicBuildingUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("WarehouseUpgrade")?.HasElements ?? false) {
        this.WarehouseUpgrade = asset.Element("WarehouseUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("GlobalUpgrade")?.HasElements ?? false) {
        this.GlobalUpgrade = asset.Element("GlobalUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("PrestigeItem")?.HasElements ?? false) {
        this.PrestigeItem = asset.Element("PrestigeItem").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("Document")?.HasElements ?? false) {
        this.Document = asset.Element("Document").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("MilitaryUnitUpgrade")?.HasElements ?? false) {
        this.MilitaryUnitUpgrade = asset.Element("MilitaryUnitUpgrade").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("TollBalancing")?.HasElements ?? false) {
        this.TollBalancing = asset.Element("TollBalancing").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("Seed")?.HasElements ?? false) {
        this.Seed = asset.Element("Seed").Elements().Select(s => new Upgrade(s)).ToList();
      }
      if (asset.Element("Sources")?.HasElements ?? false) {
        this.Sources = asset.Element("Sources").Elements().Select(s => new Upgrade(s)).ToList();
      }
    }

    #endregion Constructors
  }
}