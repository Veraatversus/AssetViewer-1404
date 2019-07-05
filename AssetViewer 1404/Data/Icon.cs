using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace AssetViewer1404.Data {

  public class Icon {

    #region Properties

    public string Filename { get; set; }

    public int FileIndex { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    #endregion Properties

    #region Constructors

    public Int32Rect GetPosition(BitmapImage pic) {
      var picsInRow = pic.PixelWidth / Width;
      var row = FileIndex / picsInRow;
      var position = FileIndex % picsInRow;

      var rowsinpic = pic.PixelHeight / Height;
      //var picwidth = pic.Width / picsInRow;
      //var picheight = pic.Height / rowsinpic;
      //var rect = new Int32Rect((int)(row * picheight), (int)(position * picwidth), (int)picwidth, (int)picheight);
      var rect = new Int32Rect(position * Width, row * Height, Width, Height);
      return rect;
    }
    public Icon(XElement item) {
      if (!string.IsNullOrEmpty(item.Element("Filename")?.Value)) {
        this.Filename = $"pack://application:,,,/AssetViewer1404;component/Resources/{item.Element("Filename").Value}";
      }
      FileIndex = int.Parse(item.Attribute("FileIndex").Value);
      Width = int.Parse(item.Attribute("Width").Value);
      Height = int.Parse(item.Attribute("Width").Value);
    }
  }

  #endregion Constructors
}