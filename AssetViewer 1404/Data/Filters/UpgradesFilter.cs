﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AssetViewer1404.Data.Filters {

  public class UpgradesFilter : BaseFilter<string> {

    #region Properties

    public override Func<IQueryable<Asset>, IQueryable<Asset>> FilterFunc => result => {
      if (!String.IsNullOrEmpty(SelectedValue))
        if (ComparisonType != FilterType.None && !String.IsNullOrEmpty(SelectedComparisonValue)) {
          result = result.Where(w => w.AllUpgrades != null && w.AllUpgrades.Where(l => l.Text != null && l.Text.CurrentLang == SelectedValue).Any(l => CompareToUpgrade(l)));
        }
        else {
          result = result.Where(w => w.AllUpgrades != null && w.AllUpgrades.Any(l => l.Text != null && l.Text.CurrentLang == SelectedValue));
        }

      return result;
    };

    public override IEnumerable<String> CurrentValues => ItemsHolder.GetResultWithoutFilter(this).SelectMany(s => s.AllUpgrades).Select(s => s.Text == null ? "" : s.Text.CurrentLang).Distinct().Where(l => !string.IsNullOrWhiteSpace(l)).Concat(new[] { string.Empty }).OrderBy(o => o)
         .ToList();

    public override IEnumerable<string> ComparisonValues => (new[] { string.Empty }).Concat(ItemsHolder
         .GetResultWithoutFilter(this)
         .SelectMany(s => s.AllUpgrades.Where(u => u.Text.CurrentLang == SelectedValue))
         .Select(u => u.Value).Where(l => !string.IsNullOrWhiteSpace(l))
         .Distinct()
         .Where(l => !string.IsNullOrWhiteSpace(l))
         .OrderBy(o => float.Parse(o.TrimEnd(' ', '%'))))
         .ToList();

    public override int DescriptionID => 8;

    #endregion Properties

    #region Constructors

    public UpgradesFilter(ItemsHolder itemsHolder) : base(itemsHolder) {
      ComparisonType = FilterType.Selection;
    }

    #endregion Constructors

    #region Methods

    private bool CompareToUpgrade(Upgrade l) {
      if (float.TryParse(l.Value.TrimEnd(' ', '%'), out var x) && float.TryParse(this.SelectedComparisonValue.TrimEnd(' ', '%'), out var y)) {
        switch (Comparison) {
          case ValueComparisons.Equals:
            return x == y;

          case ValueComparisons.LesserThan:
            return x <= y;

          case ValueComparisons.GraterThan:
            return x >= y;
        }
      }
      else {
        var stringCompare = l.Value.CompareTo(this.SelectedValue);
        switch (stringCompare) {
          case -1:
            return Comparison == ValueComparisons.LesserThan;

          case 0:
            return true;

          case 1:
            return Comparison == ValueComparisons.GraterThan;
        }
      }
      return false;
    }

    #endregion Methods
  }
}