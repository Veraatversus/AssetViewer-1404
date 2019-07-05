using System;
using System.Linq;

namespace AssetViewer1404.Data.Filters {

  public class SearchTextFilter : BaseFilter<string> {

    #region Properties

    public override string SelectedValue {
      get {
        return _selectedValue;
      }
      set {
        if (_selectedValue != value) {
          _selectedValue = value;
          RaisePropertyChanged();
          ItemsHolder.UpdateUI(this);
        }
      }
    }

    public override Func<IQueryable<Asset>, IQueryable<Asset>> FilterFunc => result => {
      if (!String.IsNullOrEmpty(SelectedValue))
        result = result.Where(w => w.ID.StartsWith(SelectedValue, StringComparison.InvariantCultureIgnoreCase) || w.Text.CurrentLang.IndexOf(SelectedValue, StringComparison.CurrentCultureIgnoreCase) >= 0);
      return result;
    };

    public override int DescriptionID => 6;

    #endregion Properties

    #region Constructors

    public SearchTextFilter(ItemsHolder itemsHolder) : base(itemsHolder) {
      FilterType = FilterType.Text;
    }

    #endregion Constructors

    #region Fields

    private string _selectedValue;

    #endregion Fields
  }
}