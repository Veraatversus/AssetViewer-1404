using RDA.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RDA {
  internal class Engine {
    internal static String PathRoot;

    public static Dictionary<string, XElement> Features { get; private set; } = new Dictionary<string, XElement>();
    public static Dictionary<string, XElement> Game { get; private set; } = new Dictionary<string, XElement>();

    internal static String PathViewer;
    public static Dictionary<String, Dictionary<Languages, (string text, List<string> subtext)>> Text { get; set; } = new Dictionary<string, Dictionary<Languages, (string, List<string>)>>();
    //public static XElement TextEditor { get; private set; }
    public static XElement DataSets { get; private set; }
    public static Dictionary<string, XElement> IconFilemap { get; private set; }
    public static Dictionary<string, XElement> Icons { get; private set; }
    public static Dictionary<string, string> NamesToId { get; private set; } = new Dictionary<string, string>();
    public static Dictionary<string, string> IdToCategory { get; private set; } = new Dictionary<string, string>();
    public static void Init() {
      Console.WriteLine("Load Asset.xml");
      PathViewer = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", String.Empty)).Parent.Parent.Parent.FullName + @"\AssetViewer 2070";
      PathRoot = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", String.Empty)).Parent.Parent.FullName;
      Features["Release"] = LoadFeatures(Engine.PathRoot + @"\Original\features\");
      Features["dlc1"] = LoadFeatures(Engine.PathRoot + @"\Original\dlc_01\");
      Features["dlc2"] = LoadFeatures(Engine.PathRoot + @"\Original\dlc_02\");
      Features["dlc3"] = LoadFeatures(Engine.PathRoot + @"\Original\dlc_03\");
      Features["dlc4"] = LoadFeatures(Engine.PathRoot + @"\Original\dlc_04\");
      Features["addon1"] = LoadFeatures(Engine.PathRoot + @"\Original\addondata\features\");
      Game["assets"] = XmlLoader.LoadXml(Engine.PathRoot + @"\Original\game\assets.xml");
      Game["conquestchapter"] = XmlLoader.LoadXml(Engine.PathRoot + @"\Original\game\conquestchapter.xml");

      //Ai = XmlLoader.LoadXml(PathRoot + @"\Original\ai\aiprofiles.xml");
      //Game = XmlLoader.LoadXml(PathRoot + @"\Original\game\assets.xml");
      //QuestConfig = XmlLoader.LoadXml(PathRoot + @"\Original\questconfig\quests.xml");
      //TextEditor = XmlLoader.LoadXml(PathRoot + @"\Original\texteditor\guids.xml");
      DataSets = XDocument.Load(PathRoot + @"\Original\game\datasets.xml").Root;
      ProcessingCaterogies();
      ProcessingTextEditorFolder();

      ProcessingCustomText();
      ProcessingIcons();
      
    }

    private static void ProcessingCaterogies() {
      var group = Game.Values.Descendants("Group").FirstOrDefault(f => f.Element("Name")?.Value == "-AssetCategory");
      foreach (var item in group.Descendants("Group").Where(e=> e.Element("GUID") != null && e.Element("AssetCategory") != null)) {
        IdToCategory.Add(item.Element("GUID").Value, item.Element("AssetCategory").Value);
      }
    }

    private static void ProcessingTextEditorFolder() {
      foreach (var item in Directory.EnumerateFiles(PathRoot + @"\Original\texteditor\")) {
        ProcessingText(item);
      }
      foreach (var item in Directory.EnumerateFiles(PathRoot + @"\Original\addondata\texteditor\")) {
        ProcessingText(item);
      }
      ProcessingText(PathRoot + @"\Original\dlc_01\texts.xml");
      ProcessingText(PathRoot + @"\Original\dlc_02\texts.xml");
      ProcessingText(PathRoot + @"\Original\dlc_03\texts.xml");
      ProcessingText(PathRoot + @"\Original\dlc_04\texts.xml");


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
      foreach (var item in textarrays.Element("ItemTypesOLD").Elements()) {
        NamesToId.Add(item.Name.LocalName, item.Element("ItemTypeText")?.Value ?? item.Element("ItemTypeDescription")?.Value);
      }
      foreach (var item in textarrays.Element("AssetCategoryNames").Elements()) {
        NamesToId.Add(item.Name.LocalName, item.Element("Text").Value);
      }

      textarrays = balancing.XPathSelectElement("DefaultValues/GUIBalancing");
      //foreach (var item in textarrays.Element("BuildmenuCategoryTextGUID").Elements()) {
      //  NamesToId.Add(item.Name.LocalName, item.Value);
      //}
      
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
      //NamesToId.Add("ProductionBuildingsOrient", "137641");
      //NamesToId.Add("MilitaryArmy", "137733");
      //NamesToId.Add("MilitaryFortification", "137730");
      //NamesToId.Add("MinTradeAmountForTollGeneration", "137561");
      //NamesToId.Add("TollBalancing", "137560");      //"137697"
      //NamesToId.Add("TollMoney", "140006");
      //NamesToId.Add("ResidenceUpgradeAmountMaxPercent", "137743");
      //NamesToId.Add("GeneratedFertility", "137532");
    }
    private static void ProcessingIcons() {
      Console.WriteLine("Processing: Icons");
      IconFilemap = XmlLoader.LoadXml(PathRoot + @"\Original\gui\iconfilemap.xml").Descendants("IconFile").ToDictionary(i => i.Element("IconFileID").Value);
      Icons = XmlLoader.LoadXml(PathRoot + @"\Original\game\icons.xml").Elements().ToDictionary(i => i.Element("GUID").Value);
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
    private static XElement LoadFeatures(string path) {
      var result = new XElement("root");
      if (File.Exists(Path.Combine(path, "features.xml"))) {
        result.Add(XmlLoader.LoadXml(Path.Combine(path, "features.xml")));
      }
      if (File.Exists(Path.Combine(path, "items.xml"))) {
        result.Add(XmlLoader.LoadXml(Path.Combine(path, "items.xml")));
      }
      return result;
    }
  }

}
