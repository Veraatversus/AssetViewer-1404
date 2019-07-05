using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RDA {
  public static class Extensions {
    public static Regex TagRegex { get; set; } = new Regex("(\\[.*?\\])", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    public static IEnumerable<string> RemoveTags(this IEnumerable<string> list) {
      return list.Select(item => {
        var matches = TagRegex.Matches(item).Cast<Match>().Select(m => m.Value).Distinct().OrderByDescending(s=> s.Length);
          foreach (var tag in matches) {
            item = item.Replace(tag, "");
          }
        return item;
      });
     
    }
    public static string RemoveTags(this string item) {
        var matches = TagRegex.Matches(item).Cast<Match>().Select(m => m.Value).Distinct().OrderByDescending(s => s.Length);
        foreach (var tag in matches) {
          item = item.Replace(tag, "");
        }
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
  }
}
