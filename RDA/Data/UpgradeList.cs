using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RDA.Data {

  [XmlRoot("Upgrades")]
  public class UpgradeList : ICollection<Upgrade> {

    #region Properties

    [XmlArray]
    public List<Upgrade> Upgrade { get; set; } = new List<Upgrade>();

    public int Count => ((ICollection<Upgrade>)Upgrade).Count;

    public bool IsReadOnly => ((ICollection<Upgrade>)Upgrade).IsReadOnly;

    #endregion Properties

    #region Methods

    public void Add(Upgrade item) {
      ((ICollection<Upgrade>)Upgrade).Add(item);
    }

    public void Clear() {
      ((ICollection<Upgrade>)Upgrade).Clear();
    }

    public bool Contains(Upgrade item) {
      return ((ICollection<Upgrade>)Upgrade).Contains(item);
    }

    public void CopyTo(Upgrade[] array, int arrayIndex) {
      ((ICollection<Upgrade>)Upgrade).CopyTo(array, arrayIndex);
    }

    public IEnumerator<Upgrade> GetEnumerator() {
      return ((ICollection<Upgrade>)Upgrade).GetEnumerator();
    }

    public bool Remove(Upgrade item) {
      return ((ICollection<Upgrade>)Upgrade).Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return ((ICollection<Upgrade>)Upgrade).GetEnumerator();
    }

    #endregion Methods
  }
}