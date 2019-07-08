using AssetViewer1404.Data;
using System.Collections.Generic;
using System.Linq;

namespace AssetViewer1404 {

  public static class DescriptionExtensions {

    #region Methods

    public static Description Join(this IEnumerable<Description> descs, string seperator = "") {
      return new Description(string.Join(seperator, descs.Select(d => d.EN)), string.Join(seperator, descs.Select(d => d.DE)));
    }

    #endregion Methods
  }
}