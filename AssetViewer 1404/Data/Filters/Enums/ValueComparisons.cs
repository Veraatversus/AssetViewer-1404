using AssetViewer1404.Converter;
using System.ComponentModel;

namespace AssetViewer1404 {

  [TypeConverter(typeof(EnumDescriptionTypeConverter))]
  public enum ValueComparisons {

    [Description("=")]
    Equals,

    [Description("<")]
    LesserThan,

    [Description(">")]
    GraterThan
  }
}