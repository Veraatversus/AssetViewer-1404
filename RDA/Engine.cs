using RDA.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RDA {

  public static class Engine {

    #region Properties

    public static XElement Ai { get; private set; }
    public static Dictionary<String, Dictionary<Languages, (string text, List<string> subtext)>> Text { get; set; } = new Dictionary<string, Dictionary<Languages, (string, List<string>)>>();
    public static XElement Game { get; private set; }
    public static XElement QuestConfig { get; private set; }
    //public static XElement TextEditor { get; private set; }
    public static XElement DataSets { get; private set; }
    public static Dictionary<string, XElement> IconFilemap { get; private set; }
    public static Dictionary<string, XElement> Icons { get; private set; }
    public static Dictionary<string, string> NamesToId { get; private set; } = new Dictionary<string, string>();
    public static IEnumerable<XElement> AllXmls => new[] { Ai, Game, QuestConfig, Features/*, Balancing*/ };

    #endregion Properties

    #region Methods

    public static void Init() {
      Console.WriteLine("Load Asset.xml");
      PathViewer = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", String.Empty)).Parent.Parent.Parent.FullName + @"\AssetViewer 1404";
      PathRoot = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", String.Empty)).Parent.Parent.FullName;
      Features = XmlLoader.LoadXml(PathRoot + @"\Original\features\features.xml");
      //Balancing = XmlLoader.LoadXml(PathRoot + @"\Original\features\balancing.xml");
      Ai = XmlLoader.LoadXml(PathRoot + @"\Original\ai\aiprofiles.xml");
      Game = XmlLoader.LoadXml(PathRoot + @"\Original\game\assets.xml");
      QuestConfig = XmlLoader.LoadXml(PathRoot + @"\Original\questconfig\quests.xml");
      //TextEditor = XmlLoader.LoadXml(PathRoot + @"\Original\texteditor\guids.xml");
      DataSets = XDocument.Load(PathRoot + @"\Original\game\datasets.xml").Root;

      ProcessingTextEditorFolder();

      ProcessingCustomText();
      ProcessingIcons();
    }

    #endregion Methods

    #region Fields

    internal static XElement Features;

    public static XElement Balancing { get; private set; }

    internal static String PathRoot;
    internal static String PathViewer;

    #endregion Fields

    private static void ProcessingTextEditorFolder() {
      foreach (var item in Directory.EnumerateFiles(PathRoot + @"\Original\texteditor\", "*", SearchOption.AllDirectories).OrderBy(f => f)) {
        ProcessingText(item);
      }
    }

    private static void ProcessingCustomText() {
      // String Keys
      Console.WriteLine("Processing : Custom Texts");
      var gameproperties = XDocument.Load(PathRoot + @"\Original\game\properties.xml");
      //Defaultvalues
      var balancing = gameproperties
         .Root
         .Descendants("Group")
         .FirstOrDefault(g => g.Element("Name")?.Value == "Balancing");

      var textarrays = balancing.XPathSelectElement("DefaultValues/GUIBalancing/ItemTooltipConfig");
      foreach (var item in textarrays.Element("TextParts").Elements()) {
        NamesToId.Add(item.Name.LocalName, item.Element("TextPart").Value);
      }
      foreach (var item in textarrays.Element("ItemTypes").Elements()) {
        NamesToId.Add(item.Name.LocalName, item.Element("ItemTypeText")?.Value ?? item.Element("ItemTypeDescription")?.Value);
      }
      foreach (var item in textarrays.Element("TargetCategories").Elements()) {
        NamesToId.Add(item.Name.LocalName, item.Element("CategoryName").Value);
      }

      textarrays = balancing.XPathSelectElement("DefaultValues/GUIBalancing");
      foreach (var item in textarrays.Element("BuildmenuCategoryTextGUID").Elements()) {
        NamesToId.Add(item.Name.LocalName, item.Value);
      }
      foreach (var item in textarrays.Element("MenuButtonsNavigationPointTextGUID").Elements()) {
        NamesToId.Add(item.Name.LocalName, item.Value);
      }
      foreach (var item in textarrays.Element("ManufactoryTypeTextGUID").Elements()) {
        NamesToId.Add(item.Name.LocalName, item.Value);
      }
      foreach (var item in textarrays.Element("ProductIconGUID").Elements()) {
        NamesToId.Add(item.Name.LocalName, item.Value);
      }
      foreach (var item in textarrays.Element("DiplomacyStatus").Elements()) {
        NamesToId.Add(item.Name.LocalName, item.Element("StatusName").Value);
      }
      foreach (var item in textarrays.Element("FertilityIconGUID").Elements()) {
        if (!NamesToId.ContainsKey(item.Name.LocalName)) {
          NamesToId.Add(item.Name.LocalName, item.Value);
        }
      }
      //Text.Add("AllBuildingsIsland", "137533");
      //Text.Add("Ships", "137503");
      //Text.Add("SouthernIslandResource", "137504");
      //Text.Add("NorthernIslandResource", "137505");
      //Text.Add("TradeBuildings", "137506");
      //Text.Add("ClothingChainBuildings", "137507");
      //Text.Add("FoodChainBuildings", "137508");
      //Text.Add("DrinkChainBuildings", "137509");
      //Text.Add("BuildingmaterialChainBuildings", "137510");
      //Text.Add("PropertyChainBuildings", "");
      //Text.Add("CommunityBuildings", "");
      //Text.Add("SafetyBuildings", "");
      //Text.Add("FaithBuildings", "137534");
      //Text.Add("AmusementBuildings", "");
      //Text.Add("MilitaryBuildings", "");
      //Text.Add("MonumentBuildings", "");
      //Text.Add("HarbourBuildings", "");
      //Text.Add("MilitaryNavyChainBuildings", "");
      //Text.Add("ResidentHouses", "");
      //Text.Add("ProductionBuildings", "");
      //Text.Add("DemandBuildings", "");
      //Text.Add("FoodChainEndBuildings", "");
      //Text.Add("DrinkChainEndBuildings", "");
      //Text.Add("ClothChainEndBuildings", "");
      //Text.Add("PropertyChainEndBuildings", "");
      //Text.Add("BuildingMaterialChainEndBuildings", "");
      //Text.Add("MilitaryNavyChainEndBuildings", "");
      //Text.Add("SpecialBuildings", "");
      //Text.Add("StorageBuildings", "");
      //Text.Add("MilitaryArmy", "");
      //Text.Add("MilitaryFortification", "");
      NamesToId.Add("ProductionBuildingsOrient", "137641");
      NamesToId.Add("MilitaryArmy", "137733");
      NamesToId.Add("MilitaryFortification", "137730");
      NamesToId.Add("MinTradeAmountForTollGeneration", "137561");
      NamesToId.Add("TollBalancing", "137560");      //"137697"
      NamesToId.Add("TollMoney", "140006");
      NamesToId.Add("ResidenceUpgradeAmountMaxPercent", "137743");
      NamesToId.Add("GeneratedFertility", "137532");
      NamesToId.Add("DiscoverAllChance", "137570");
      NamesToId.Add("SharePriceForeignUpgrade", "40122013");
      NamesToId.Add("SharePriceOwnerUpgrade", "40122012");
      NamesToId.Add("TownAcquisitionCooldown", "40122011");
      NamesToId.Add("WaterCapacity", "40122007");
      NamesToId.Add("ProductionCapacity", "40122007");
      NamesToId.Add("IncreasedMaintenanceCost", "40122005");
    }

    private static void ProcessingIcons() {
      Console.WriteLine("Processing: Icons");
      IconFilemap = XmlLoader.LoadXml(PathRoot + @"\Original\gui\iconfilemap.xml").Descendants("IconFile").ToDictionary(i => i.Element("IconFileID").Value);
      Icons = XmlLoader.LoadXml(PathRoot + @"\Original\gui\icons.xml").Elements().ToDictionary(i => i.Element("GUID").Value);
      foreach (var item in DataSets.Descendants("Item").Where(i => i.Element("Icon") != null)) {
        string id = item.Element("Name").Value;
        if (NamesToId.ContainsKey(item.Element("Name").Value)) {
          id = NamesToId[item.Element("Name").Value];
        }
        if (!Icons.ContainsKey(id)) {
          Icons.Add(id, item);
        }
        else {
        }
      }
    }

    private static void ProcessingText(String path) {
      //GUID Keys
      Console.WriteLine("Processing: " + path);
      var element = XDocument.Load(path).Root;
      var values = element.Descendants("Asset");
      foreach (var value in values) {
        var id = value.XPathSelectElement("Values/Standard/GUID").Value;
        if (!Text.ContainsKey(id)) {
          var languages = new Dictionary<Languages, (string, List<string>)>();
          foreach (var item in value.XPathSelectElement("Values/Text/LocaText")?.Elements() ?? Enumerable.Empty<XElement>()) {
            if (item.Element("Text") != null) {
              (string text, List<string> subtexts) tuble;
              tuble.text = item.Element("Text").Value.RemoveTags();
              tuble.subtexts = item
                .Parent
                .Parent
                .Parent
                .Parent
                .Element("SubAssets")
                ?.Descendants(item.Name.LocalName)
                .Where(e => e.Element("Text") != null)
                .Select(e => e.Element("Text").Value)
                .RemoveTags()
                .ToList();
              languages[(Languages)Enum.Parse(typeof(Languages), item.Name.LocalName)] = tuble;
            }
          }
          if (languages != null)
            Text.Add(id, languages);
        }
      }
    }
  }
}