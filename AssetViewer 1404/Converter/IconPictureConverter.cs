using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Icon = AssetViewer1404.Data.Icon;

namespace AssetViewer1404.Converter {

  [ValueConversion(typeof(Icon), typeof(ImageSource))]
  public class IconPictureConverter : IValueConverter {

    #region Fields

    public static Dictionary<string, BitmapImage> Images = new Dictionary<string, BitmapImage>();

    #endregion Fields

    #region Methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var icon = value as Icon;
      BitmapImage bi = null;
      if (Images.ContainsKey(icon.Filename)) {
        bi = Images[icon.Filename];
      }
      else {
        bi = new BitmapImage();
        bi.BeginInit();
        bi.UriSource = new Uri(icon.Filename, UriKind.Absolute);
        bi.EndInit();
        Images.Add(icon.Filename, bi);
      }
      return new CroppedBitmap(bi, icon.GetPosition(bi));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }

    #endregion Methods
  }
}