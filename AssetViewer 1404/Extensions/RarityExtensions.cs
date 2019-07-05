using AssetViewer1404.Data;

namespace AssetViewer1404.Extensions {

  public static class RarityExtensions {

    #region Methods

    public static Description RarityToDescription(string rarity) {
      switch (rarity) {
        case "C":
          return App.Descriptions[102];

        case "B":
          return App.Descriptions[101];

        default:
          return App.Descriptions[100];
      }
    }

    #endregion Methods
  }
}