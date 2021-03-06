﻿using AssetViewer1404.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace AssetViewer1404 {

  /// <summary>
  /// Interaktionslogik für "App.xaml"
  /// </summary>
  public partial class App : Application {

    #region Properties

    public static Languages Language { get; internal set; }

    #endregion Properties

    #region Fields

    public static Dictionary<int, Description> Descriptions = new Dictionary<int, Description> {
      [1] = new Description("German", "Deutsch"),
      [2] = new Description("English", "Englisch"),
      [3] = new Description("Items", "Gegenstände"),
      [4] = new Description("Mode", "Modus"),
      [5] = new Description("Costs: ", "Preis: "),
      [6] = new Description("Search", "Suchen"),
      [7] = new Description("Rarity", "Seltenheit"),
      [8] = new Description("Attribute", "Eigenschaft"),
      [9] = new Description("Sort by", "Sortieren nach"),
      [10] = new Description("Reset Filters", "Filter zurücksetzen"),
      [11] = new Description("Source", "Quelle"),
      [12] = new Description("Only available items ", "Nur verfügbare Items "),
      [13] = new Description("Sort by", "Sortieren nach"),
      [14] = new Description("Affect Building", "Beeinflusst Gebäude"),
      [15] = new Description("Item Category", "Item Kategorie"),
      [100] = new Description("Common", "Gewöhnlich"),
      [101] = new Description("Rare", "Selten"),
      [102] = new Description("Epic", "Episch"),
    };

    #endregion Fields

    #region Constructors

    public App() {
      if (CultureInfo.CurrentCulture.ThreeLetterWindowsLanguageName == "DEU") {
        Language = Languages.German;
      }
    }

    #endregion Constructors
  }
}