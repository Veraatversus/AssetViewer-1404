using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml2CSharp {

  [XmlRoot(ElementName = "Asset")]
  public class Asset {

    #region Properties

    [XmlElement(ElementName = "AffectTargets")]
    public string AffectTargets { get; set; }

    [XmlElement(ElementName = "HonourPrice")]
    public HonourPrice HonourPrice { get; set; }

    [XmlAttribute(AttributeName = "ID")]
    public string ID { get; set; }

    [XmlElement(ElementName = "ItemCategory")]
    public string ItemCategory { get; set; }

    [XmlElement(ElementName = "ItemQuality")]
    public string ItemQuality { get; set; }

    [XmlElement(ElementName = "ItemTarget")]
    public string ItemTarget { get; set; }

    [XmlElement(ElementName = "ItemType")]
    public string ItemType { get; set; }

    [XmlAttribute(AttributeName = "Name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "ShipCombatUpgrade")]
    public ShipCombatUpgrade ShipCombatUpgrade { get; set; }

    [XmlElement(ElementName = "TargetCategories")]
    public string TargetCategories { get; set; }

    [XmlElement(ElementName = "Text")]
    public Text Text { get; set; }

    [XmlElement(ElementName = "Upgrade")]
    public string Upgrade { get; set; }

    #endregion Properties
  }

  [XmlRoot(ElementName = "HonourPrice")]
  public class HonourPrice {

    #region Properties

    [XmlElement(ElementName = "DE")]
    public string DE { get; set; }

    [XmlElement(ElementName = "EN")]
    public string EN { get; set; }

    [XmlAttribute(AttributeName = "FontStyle")]
    public string FontStyle { get; set; }

    [XmlElement(ElementName = "Icon")]
    public Icon Icon { get; set; }

    [XmlAttribute(AttributeName = "ID")]
    public string ID { get; set; }

    #endregion Properties
  }

  [XmlRoot(ElementName = "Icon")]
  public class Icon {

    #region Properties

    [XmlAttribute(AttributeName = "FileIndex")]
    public string FileIndex { get; set; }

    [XmlElement(ElementName = "Filename")]
    public string Filename { get; set; }

    [XmlAttribute(AttributeName = "Height")]
    public string Height { get; set; }

    [XmlAttribute(AttributeName = "Width")]
    public string Width { get; set; }

    #endregion Properties
  }

  [XmlRoot(ElementName = "ShipCombatUpgrade")]
  public class ShipCombatUpgrade {

    #region Properties

    [XmlElement(ElementName = "UpgradeList")]
    public UpgradeList UpgradeList { get; set; }

    #endregion Properties
  }

  [XmlRoot(ElementName = "ShipItem")]
  public class ShipItem {

    #region Properties

    [XmlElement(ElementName = "Asset")]
    public Asset Asset { get; set; }

    #endregion Properties
  }

  [XmlRoot(ElementName = "Text")]
  public class Text {

    #region Properties

    [XmlElement(ElementName = "DE")]
    public string DE { get; set; }

    [XmlElement(ElementName = "EN")]
    public string EN { get; set; }

    [XmlAttribute(AttributeName = "FontStyle")]
    public string FontStyle { get; set; }

    [XmlElement(ElementName = "Icon")]
    public Icon Icon { get; set; }

    [XmlAttribute(AttributeName = "ID")]
    public string ID { get; set; }

    #endregion Properties
  }

  [XmlRoot(ElementName = "Upgrade")]
  public class Upgrade {

    #region Properties

    [XmlElement(ElementName = "Text")]
    public Text Text { get; set; }

    [XmlElement(ElementName = "Value")]
    public string Value { get; set; }

    #endregion Properties
  }

  [XmlRoot(ElementName = "UpgradeList")]
  public class UpgradeList {

    #region Properties

    [XmlElement(ElementName = "Upgrade")]
    public List<Upgrade> Upgrade { get; set; }

    #endregion Properties
  }
}