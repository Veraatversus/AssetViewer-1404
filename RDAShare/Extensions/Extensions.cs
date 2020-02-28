using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RDA {

  public static class Extensions {

    #region Properties

    public static Regex TagRegex { get; set; } = new Regex("(\\[.*?\\])", RegexOptions.IgnoreCase | RegexOptions.Singleline);

    #endregion Properties

    #region Methods

    public static IEnumerable<string> RemoveTags(this IEnumerable<string> list) {
      return list.Select(item => {
        var matches = TagRegex.Matches(item).Cast<Match>().Select(m => m.Value).Distinct().OrderByDescending(s => s.Length);
        foreach (var tag in matches) {
          item = item.Replace(tag, "");
        }
        return item;
      });
    }
    public static string RemoveTags(this string item) {
      //var matches = TagRegex.Matches(item).Cast<Match>().Select(m => m.Value).Distinct().OrderByDescending(s => s.Length);
      //foreach (var tag in matches) {
      //  item = item.Replace(tag, "");
      //}
      item = item.Replace("[CR]", "");
      return item;
    }
    public static XElement GetProxyElement(this XElement element, string proxyName) {
      var xRoot = new XElement("Proxy");
      var xTemplate = new XElement("Template");
      xTemplate.Value = proxyName;
      xRoot.Add(xTemplate);
      xRoot.Add(element);
      xRoot.Add(new XElement("Values", element.XPathSelectElement("Values/Standard")));
      return xRoot;
    }

    #endregion Methods
  }
}