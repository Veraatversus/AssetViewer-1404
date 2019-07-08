using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AssetViewer1404.Data.Filters {

  public class OrderFilter : BaseFilter<string> {

    #region Properties

    public override Func<IQueryable<Asset>, IQueryable<Asset>> FilterFunc => result => {
      if (!String.IsNullOrEmpty(SelectedValue))
        switch (values.FirstOrDefault(v => v.DE == SelectedValue || v.EN == SelectedValue).EN) {
          case "Name":
            result = result.OrderBy(w => w.Text.CurrentLang);
            break;

          case "ID":
            result = result.OrderBy(w => int.Parse(w.ID));
            break;

          case "Rarity":
            result = result.OrderBy(w => w.ItemQuality);
            break;

          case "Honor Costs":
            result = result.OrderBy(w => string.IsNullOrWhiteSpace(w.HonourPrice.CurrentLang) ? 0 : Int32.Parse(w.HonourPrice.CurrentLang.Split(' ').FirstOrDefault() ?? "0", NumberStyles.Any));
            break;
        }

      return result;
    };

    public override IEnumerable<String> CurrentValues => values.Select(d => d.CurrentLang);

    public override int DescriptionID => 13;

    #endregion Properties

    #region Constructors

    public OrderFilter(ItemsHolder itemsHolder) : base(itemsHolder) {
      SelectedValue = values[0].CurrentLang;
    }

    #endregion Constructors

    #region Fields

    private Description[] values = new[] {
      new Description("Name", "Name"),
      new Description("ID", "ID"),
      new Description("Rarity", "Seltenheit"),
      new Description("Selling Price", "Verkaufspreis")
    };

    #endregion Fields
  }
}