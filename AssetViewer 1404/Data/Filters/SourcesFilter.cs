﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AssetViewer1404.Data.Filters {

  public class SourcesFilter : BaseFilter<string> {

    #region Properties

    public override Func<IQueryable<Asset>, IQueryable<Asset>> FilterFunc => result => {
      if (!string.IsNullOrWhiteSpace(SelectedComparisonValue)) {
        if (!string.IsNullOrWhiteSpace(SelectedValue)) {
          result = result.Where(w => w.Sources != null && w.Sources.Where(s => s.Text.CurrentLang == SelectedValue).SelectMany(s => s.Additionals).Any(l => l.Text.CurrentLang == SelectedComparisonValue));
        }
        else {
          result = result.Where(w => w.Sources != null && w.Sources.SelectMany(s => s.Additionals).Any(l => l.Text.CurrentLang == SelectedComparisonValue));
        }
      }
      else if (!String.IsNullOrEmpty(SelectedValue))
        result = result.Where(w => w.Sources != null && w.Sources.Any(l => l.Text.CurrentLang == SelectedValue));
      return result;
    };

    public override IEnumerable<String> CurrentValues => ItemsHolder
         .GetResultWithoutFilter(this)
         .SelectMany(s => s.Sources ?? Enumerable.Empty<Upgrade>())
         .Select(s => s.Text.CurrentLang)
         .Distinct()
         .Where(l => !string.IsNullOrWhiteSpace(l))
         .Concat(new[] { string.Empty })
         .OrderBy(o => o)
         .ToList();

    public override IEnumerable<string> ComparisonValues => GetComparisonValues();
    public override int DescriptionID => 11;

    #endregion Properties

    #region Constructors

    public SourcesFilter(ItemsHolder itemsHolder) : base(itemsHolder) {
      ComparisonType = FilterType.Selection;
    }

    #endregion Constructors

    #region Methods

    private IEnumerable<string> GetComparisonValues() {
      if (String.IsNullOrEmpty(SelectedValue)) {
        return ItemsHolder
        .GetResultWithoutFilter(this)
        .SelectMany(s => s.Sources ?? Enumerable.Empty<Upgrade>())
        .SelectMany(s => s.Additionals)
        .Select(s => s.Text.CurrentLang)
        .Distinct()
        .Where(l => !string.IsNullOrWhiteSpace(l))
        .Concat(new[] { string.Empty })
        .OrderBy(o => o)
        .ToList();
      }
      else {
        return ItemsHolder
        .GetResultWithoutFilter(this)
        .SelectMany(s => s.Sources ?? Enumerable.Empty<Upgrade>())
        .Where(s => s.Text.CurrentLang == SelectedValue)
        .SelectMany(s => s.Additionals)
        .Select(s => s.Text.CurrentLang)
        .Distinct()
        .Where(l => !string.IsNullOrWhiteSpace(l))
        .Concat(new[] { string.Empty })
        .OrderBy(o => o)
        .ToList();
      }
    }

    #endregion Methods
  }
}