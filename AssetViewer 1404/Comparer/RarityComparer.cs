using AssetViewer1404.Data;
using System.Collections.Generic;

namespace AssetViewer1404.Comparer {

  public class RarityComparer : IComparer<string> {

    #region Properties

    public static RarityComparer Default { get; } = new RarityComparer();

    #endregion Properties

    #region Methods

    public int Compare(string x, string y) {
      if (App.Language == Languages.German) {
        return RaritiesDE.IndexOf(x).CompareTo(RaritiesDE.IndexOf(y));
      }
      else {
        return RaritiesEN.IndexOf(x).CompareTo(RaritiesEN.IndexOf(y));
      }
    }

    #endregion Methods

    #region Fields

    private static List<string> RaritiesEN = new List<string>() { "Quest Item", "Common", "Uncommon", "Rare", "Epic", "Legendary" };
    private static List<string> RaritiesDE = new List<string>() { "Aufgaben-Item", "Gewöhnlich", "Ungewöhnlich", "Selten", "Episch", "Legendär" };

    #endregion Fields
  }
}