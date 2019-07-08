using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AssetViewer1404.Data.Filters {

  public class FilterHolder : INotifyPropertyChanged {

    #region Properties

    public IFilter SelectedFilter {
      get {
        return _selectedFilter;
      }
      set {
        if (_selectedFilter != value) {
          _selectedFilter = value;
          RaisePropertyChanged(nameof(SelectedFilter));
        }
      }
    }

    public Collection<IFilter> Filters { get; } = new Collection<IFilter>();

    #endregion Properties

    #region Constructors

    public FilterHolder(ItemsHolder holder) {
      Filters.Add(new UpgradesFilter(holder));
      Filters.Add(new SourcesFilter(holder));
      Filters.Add(new ItemCategoryFilter(holder));
      //Filters.Add(new ItemTypesFilter(holder));
      //Filters.Add(new TargetsFilter(holder));
      //Filters.Add(new ReleaseVersionsFilter(holder));
      Filters.Add(new RaritiesFilter(holder));
      Filters.Add(new AvailableFilter(holder) { SelectedValue = true });
      //Filters.Add(new EquippedFilter(holder));
      Filters.Add(new SearchTextFilter(holder));
      Filters.Add(new TargetBuildingFilter(holder));
    }

    #endregion Constructors

    #region Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Events

    #region Methods

    public void RaisePropertyChanged([CallerMemberName]string name = "") {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion Methods

    #region Fields

    private IFilter _selectedFilter;

    #endregion Fields
  }
}