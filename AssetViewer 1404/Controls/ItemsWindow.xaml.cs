using AssetViewer1404.Data;
using AssetViewer1404.Data.Filters;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace AssetViewer1404.Controls {

  /// <summary>
  /// Interaktionslogik für ItemsWindow.xaml
  /// </summary>
  public partial class ItemsWindow : UserControl, INotifyPropertyChanged {

    #region Properties

    public Asset SelectedAsset { get; set; }

    public ItemsHolder ItemsHolder { get; } = new ItemsHolder();

    public Description ResetButtonText => App.Descriptions[10];

    #endregion Properties

    #region Constructors

    public ItemsWindow() {
      InitializeComponent();
      ((MainWindow)Application.Current.MainWindow).ComboBoxLanguage.SelectionChanged += this.ComboBoxLanguage_SelectionChanged;
      ItemsHolder.SetItems();
      this.DataContext = this;
    }

    #endregion Constructors

    #region Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Events

    #region Methods

    public void RaisePropertyChanged([CallerMemberName]string name = "") {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    private void ListBoxItems_OnSelectionChanged(Object sender, SelectionChangedEventArgs e) {
      if (e.AddedItems.Count == 0)
        this.ListBoxItems.SelectedIndex = 0;
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedAsset"));
    }
    private void ComboBoxLanguage_SelectionChanged(Object sender, SelectionChangedEventArgs e) {
      switch (((ComboBox)sender).SelectedIndex) {
        case 0:
          App.Language = Languages.English;
          break;

        case 1:
          App.Language = Languages.German;
          break;
      }
      ItemsHolder.RaiseLanguageChanged();
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
    }
    private void BtnResetFilters_Click(object sender, RoutedEventArgs e) => ItemsHolder.ResetFilters();
    private void BtnAddFilter_Click(object sender, RoutedEventArgs e) {
      ItemsHolder.CustomFilters.Add(new FilterHolder(ItemsHolder));
    }

    private void BtnRemoveFilter_Click(object sender, RoutedEventArgs e) {
      var filter = (FilterHolder)(sender as Button).DataContext;
      if (ItemsHolder.CustomFilters.Contains(filter)) {
        ItemsHolder.CustomFilters.Remove(filter);
      }
    }

    private void ComboBox_SelectionChanged(object sender, EventArgs e) {
      ItemsHolder.UpdateUI();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
      ItemsHolder.IsRefreshingUi = true;

      this.ListBoxItems.SelectedIndex = 0;
      ItemsHolder.IsRefreshingUi = false;
      ItemsHolder.UpdateUI();
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
    }

    #endregion Methods
  }
}