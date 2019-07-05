using System;
using System.Windows.Markup;

namespace AssetViewer1404 {

  public class Desc : MarkupExtension {

    #region Properties

    public int ID { get; set; }

    #endregion Properties

    #region Methods

    public override object ProvideValue(IServiceProvider serviceProvider) {
      return App.Descriptions[ID];
    }

    #endregion Methods
  }
}