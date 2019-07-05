using AssetViewer1404.Comparer;
using AssetViewer1404.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AssetViewer1404.Data.Filters {

  public class RaritiesFilter : BaseFilter<string> {

    #region Properties

    public override Func<IQueryable<Asset>, IQueryable<Asset>> FilterFunc => result => {
      if (!String.IsNullOrEmpty(SelectedComparisonValue)) {
        result = result.Where(w => CompareToRarity(w.ItemQuality));
      }
      else if (!String.IsNullOrEmpty(SelectedValue)) {
        result = result.Where(w => RarityExtensions.RarityToDescription(w.ItemQuality).CurrentLang == SelectedValue);
      }
      return result;
    };

    public override IEnumerable<String> CurrentValues => ItemsHolder
         .GetResultWithoutFilter(this)
         .Select(s => RarityExtensions.RarityToDescription(s.ItemQuality).CurrentLang)
         .Distinct()
         .Where(l => !string.IsNullOrWhiteSpace(l))
         .Concat(new[] { string.Empty })
         .OrderBy(o => o, RarityComparer.Default)
         .ToList();

    public override IEnumerable<string> ComparisonValues => ItemsHolder
         .GetResultWithoutFilter(this)
         .Select(s => RarityExtensions.RarityToDescription(s.ItemQuality).CurrentLang)
         .Distinct()
         .Where(l => !string.IsNullOrWhiteSpace(l))
         .Concat(new[] { string.Empty })
         .OrderBy(o => o, RarityComparer.Default)
         .ToList();

    public override int DescriptionID => 7;

    #endregion Properties

    #region Constructors

    public RaritiesFilter(ItemsHolder itemsHolder) : base(itemsHolder) {
      ComparisonType = FilterType.Selection;
      FilterType = FilterType.None;
    }

    #endregion Constructors

    #region Methods

    private bool CompareToRarity(string l) {
      switch (Comparison) {
        case ValueComparisons.Equals:
          return RarityComparer.Default.Compare(RarityExtensions.RarityToDescription(l).CurrentLang, SelectedComparisonValue) == 0;

        case ValueComparisons.LesserThan:
          return RarityComparer.Default.Compare(RarityExtensions.RarityToDescription(l).CurrentLang, SelectedComparisonValue) <= 0;

        case ValueComparisons.GraterThan:
          return RarityComparer.Default.Compare(RarityExtensions.RarityToDescription(l).CurrentLang, SelectedComparisonValue) >= 0;
      }
      return false;
    }

    #endregion Methods
  }
}