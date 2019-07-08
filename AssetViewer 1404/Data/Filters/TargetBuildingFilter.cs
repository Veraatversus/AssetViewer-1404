using System;
using System.Collections.Generic;
using System.Linq;

namespace AssetViewer1404.Data.Filters {

  public class TargetBuildingFilter : BaseFilter<string> {

    #region Properties

    public override Func<IQueryable<Asset>, IQueryable<Asset>> FilterFunc => result => {
      if (!String.IsNullOrEmpty(SelectedValue))
        result = result.Where(w => w.AffectTargets != null && w.AffectTargets/*.SelectMany(e => e..Buildings)*/.Any(s => s.CurrentLang == SelectedValue));
      return result;
    };

    public override IEnumerable<String> CurrentValues => ItemsHolder
         .GetResultWithoutFilter(this)
         .SelectMany(s => s.AffectTargets)
         //.SelectMany(s => s.Buildings)
         .Select(s => s.CurrentLang)
         .Distinct()
         .Where(l => !string.IsNullOrWhiteSpace(l))
         .Concat(new[] { string.Empty })
         .OrderBy(o => o)
         .ToList();

    public override int DescriptionID => 14;

    #endregion Properties

    #region Constructors

    public TargetBuildingFilter(ItemsHolder itemsHolder) : base(itemsHolder) {
    }

    #endregion Constructors
  }
}