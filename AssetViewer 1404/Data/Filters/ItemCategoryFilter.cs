using System;
using System.Collections.Generic;
using System.Linq;

namespace AssetViewer1404.Data.Filters {

  public class ItemCategoryFilter : BaseFilter<string> {

    #region Properties

    public override IEnumerable<string> CurrentValues => ItemsHolder
        .GetResultWithoutFilter(this)
        .Select(a => a.ItemCategory)
        .Distinct()
        .Where(s => !string.IsNullOrWhiteSpace(s))
        .Concat(new[] { string.Empty })
        .OrderBy(s => s)
        .ToList();

    public override int DescriptionID => 15;

    public override Func<IQueryable<Asset>, IQueryable<Asset>> FilterFunc => result => {
      if (!String.IsNullOrEmpty(SelectedValue)) {
        result = result.Where(a => a.ItemCategory == SelectedValue);
      }
      return result;
    };

    #endregion Properties

    #region Constructors

    public ItemCategoryFilter(ItemsHolder itemsHolder) : base(itemsHolder) {
      this.FilterType = FilterType.Selection;
    }

    #endregion Constructors
  }
}