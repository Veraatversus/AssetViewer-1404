using System;

namespace AssetViewer1404.Data {

  [Flags]
  public enum DescriptionFontStyle {
    Regular = 0,
    Bold = 1,
    Italic = 2,
    Underline = 4,
    Strikeout = 8,
    Light = 16
  }
}