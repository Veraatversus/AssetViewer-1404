using AssetViewer1404.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace AssetViewer1404 {

  public static class ItemProvider {

    #region Properties

    public static Dictionary<string, Asset> Items { get; } = new Dictionary<string, Asset>();

    #endregion Properties

    //public static Dictionary<string, Pool> Pools { get; } = new Dictionary<string, Pool>();

    #region Constructors

    static ItemProvider() {
      //  using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AssetViewer.Resources.Assets.RewardPools.xml"))
      //  using (var reader = new StreamReader(stream)) {
      //    var document = XDocument.Parse(reader.ReadToEnd()).Root;
      //    foreach (var item in document.Elements().Select(s => s.FromXElement<Pool>())) {
      //      Pools.Add(item.ID, item);
      //    }
      //  }

      var arr = new[] {
                "AssetViewer1404.Resources.Assets.BoardersItem.xml",
                "AssetViewer1404.Resources.Assets.ConstructionItem.xml",
                "AssetViewer1404.Resources.Assets.DocumentItem.xml",
                "AssetViewer1404.Resources.Assets.ExpeditionItem.xml",
                "AssetViewer1404.Resources.Assets.ExplosionItem.xml",
                "AssetViewer1404.Resources.Assets.ForceTreatyItem.xml",
                "AssetViewer1404.Resources.Assets.IslandProductionBoostItem.xml",
                "AssetViewer1404.Resources.Assets.LetterOfMarqueItem.xml",
                "AssetViewer1404.Resources.Assets.MilitaryItem.xml",
                "AssetViewer1404.Resources.Assets.PetItem.xml",
                "AssetViewer1404.Resources.Assets.PopulationItem.xml",
                "AssetViewer1404.Resources.Assets.PrestigeItem.xml",
                "AssetViewer1404.Resources.Assets.QuestItem.xml",
                "AssetViewer1404.Resources.Assets.QuestUnitItem.xml",
                "AssetViewer1404.Resources.Assets.RepairShipItem.xml",
                "AssetViewer1404.Resources.Assets.SeaChartItem.xml",
                "AssetViewer1404.Resources.Assets.SeedsItem.xml",
                "AssetViewer1404.Resources.Assets.ShipItem.xml",
                "AssetViewer1404.Resources.Assets.ShroudOfSmokeItem.xml",
                "AssetViewer1404.Resources.Assets.TollItem.xml",
                "AssetViewer1404.Resources.Assets.TrashItem.xml",
                "AssetViewer1404.Resources.Assets.WarehouseItem.xml",
                "AssetViewer1404.Resources.Assets.WhiteFlagItem.xml",
                "AssetViewer1404.Resources.Assets.CustomResourceItem.xml",
                "AssetViewer1404.Resources.Assets.DoctorItem.xml",
                "AssetViewer1404.Resources.Assets.EndlessResourceItem.xml",
                "AssetViewer1404.Resources.Assets.SpyBaseDetectionItem.xml",
                "AssetViewer1404.Resources.Assets.SpyBaseItem.xml"
            };

      foreach (var str in arr) {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(str))
        using (var reader = new StreamReader(stream)) {
          var document = XDocument.Parse(reader.ReadToEnd()).Root;
          foreach (var item in document.Elements().Select(a => new Asset(a))) {
            if (!Items.ContainsKey(item.ID)) {
              Items.Add(item.ID, item);
            }
            else {
              Debug.WriteLine(item.ID);
            }
          }
        }
      }
    }

    #endregion Constructors

    #region Methods

    public static IEnumerable<Asset> GetItemsById(this IEnumerable<string> ids) {
      return ids.SelectMany(l => ItemProvider.GetItemsById(l)).Distinct() ?? Enumerable.Empty<Asset>();
    }

    public static IEnumerable<Asset> GetItemsById(this string id) {
      foreach (var item in SearchItems(id).Distinct()) {
        yield return item;
      }

      IEnumerable<Asset> SearchItems(string searchid) {
        if (searchid == null) {
        }
        else if (Items.ContainsKey(searchid)) {
          yield return Items[searchid];
        }
        //else if (Pools.ContainsKey(searchid)) {
        //  foreach (var item in Pools[searchid].Items) {
        //    if (item.Weight > 0) {
        //      foreach (var item2 in SearchItems(item.ID)) {
        //        yield return item2;
        //      }
        //    }
        //  }
        //}
        else {
          Debug.WriteLine(searchid);
        }
      }
    }

    #endregion Methods
  }
}