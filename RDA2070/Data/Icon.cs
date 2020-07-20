using System;
using System.IO;
using System.Linq;

namespace RDA.Data {

  [Serializable]
  public class Icon : RDA.Share.IconBase {

    #region Constructors

    public Icon() {
    }
    public Icon(string guid) {
      if (Engine.Icons.ContainsKey(guid)) {
        var icon = Engine.Icons[guid];
        FileIndex = icon.Descendants("IconIndex")?.FirstOrDefault()?.Value ?? "0";
        var iconmap = Engine.IconFilemap[icon.Descendants("IconFileID").First().Value];
        Filename = iconmap.Element("IconFilename").Value;
        Width = iconmap.Element("IconWidth").Value;
        Height = iconmap.Element("IconHeight").Value;

        var searchPath = Path.GetDirectoryName($@"{Engine.PathRoot}\Resources\{Filename}");
        var searchPattern = Path.GetFileNameWithoutExtension($@"{Engine.PathRoot}\Resources\{Filename}");

        var fileNames = Directory.GetFiles(searchPath, $"{searchPattern}??.png", SearchOption.TopDirectoryOnly);
        if (fileNames.Length != 1)
          throw new FileNotFoundException();

        var file = File.ReadAllBytes(fileNames[0]);
        var targetPath = Path.GetDirectoryName($@"{Engine.PathViewer}\Resources\{Filename}");
        var targetFile = Path.GetFullPath($@"{Engine.PathViewer}\Resources\{Filename}");
        if (!Directory.Exists(targetPath))
          Directory.CreateDirectory(targetPath);
        if (!File.Exists(targetFile)) {
          try {
            File.WriteAllBytes(targetFile, file);
          }
          catch (Exception) { }
        }
      }
    }

    #endregion Constructors
  }
}