using AssetViewer1404.Data;
using AssetViewer1404.Extensions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AssetViewer1404.Controls {

  /// <summary>
  /// Interaktionslogik für ItemCard.xaml
  /// </summary>
  public partial class ItemCard : UserControl, INotifyPropertyChanged {

    #region Properties

    public bool CanSwap {
      get { return (bool)GetValue(CanSwapProperty); }
      set { SetValue(CanSwapProperty, value); }
    }

    public Asset SelectedAsset {
      get { return (Asset)GetValue(SelectedAssetProperty); }
      set { SetValue(SelectedAssetProperty, value); }
    }

    public Description ItemQualityString => GetItemQuailityString();

    public LinearGradientBrush RarityBrush {
      get {
        var selection = this.SelectedAsset?.ItemQuality ?? "A";
        switch (selection) {
          case "C":
            return new LinearGradientBrush(new GradientStopCollection {
              new GradientStop(Colors.Purple, 0),          //Color.FromRgb(65, 89, 41)
              new GradientStop(Color.FromRgb(42, 44, 39), 0.2),
              new GradientStop(Color.FromRgb(42, 44, 39), 1)
            }, 90);

          case "B":
            return new LinearGradientBrush(new GradientStopCollection {
              new GradientStop(Colors.Blue, 0),         //Color.FromRgb(50, 60, 83)
              new GradientStop(Color.FromRgb(42, 44, 39), 0.2),
              new GradientStop(Color.FromRgb(42, 44, 39), 1)
            }, 90);

          default:
            return new LinearGradientBrush(new GradientStopCollection {
              new GradientStop(Colors.Green, 0),  //Color.FromRgb(126, 128, 125)
              new GradientStop(Color.FromRgb(42, 44, 39), 0.2),
              new GradientStop(Color.FromRgb(42, 44, 39), 1)
            }, 90);
        }
      }
    }

    #endregion Properties

    #region Fields

    public static readonly DependencyProperty CanSwapProperty =
                                                 DependencyProperty.Register("CanSwap", typeof(bool), typeof(ItemCard), new PropertyMetadata(false));

    public static readonly DependencyProperty SelectedAssetProperty =
        DependencyProperty.Register("SelectedAsset", typeof(Asset), typeof(ItemCard), new PropertyMetadata(null, OnSelectedAssetChanged));

    #endregion Fields

    #region Constructors

    public ItemCard() {
      InitializeComponent();
      Loaded += ItemCard_Loaded;
      Unloaded += ItemCard_Unloaded;
    }

    #endregion Constructors

    #region Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Events

    #region Methods

    public void RaisePropertyChanged([CallerMemberName]string name = "") {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    private static void OnSelectedAssetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      (d as ItemCard).RaisePropertyChanged(nameof(RarityBrush));
      (d as ItemCard).RaisePropertyChanged(nameof(ItemQualityString));
    }
    private Description GetItemQuailityString() {
      var selection = this.SelectedAsset?.ItemQuality ?? "A";
      return RarityExtensions.RarityToDescription(selection);
    }
    private void ItemCard_Unloaded(object sender, RoutedEventArgs e) {
      if (Application.Current.MainWindow is MainWindow main) {
        main.ComboBoxLanguage.SelectionChanged -= this.ComboBoxLanguage_SelectionChanged;
      }
    }

    private void ItemCard_Loaded(object sender, RoutedEventArgs e) {
      if (Application.Current.MainWindow is MainWindow main) {
        main.ComboBoxLanguage.SelectionChanged += this.ComboBoxLanguage_SelectionChanged;
      }
    }

    private void ComboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      mainGrid.DataContext = null;
      mainGrid.DataContext = this;
    }

    private void ButtonSwitch_Click(Object sender, RoutedEventArgs e) {
      if (this.ItemFront.Visibility == Visibility.Visible) {
        this.ItemFront.Visibility = Visibility.Collapsed;
        this.ItemBack.Visibility = Visibility.Visible;
      }
      else {
        this.ItemBack.Visibility = Visibility.Collapsed;
        this.ItemFront.Visibility = Visibility.Visible;
      }
    }

    #endregion Methods
  }
}