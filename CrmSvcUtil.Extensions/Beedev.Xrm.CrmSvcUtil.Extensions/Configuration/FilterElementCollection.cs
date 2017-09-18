using System.Collections.Generic;
using System.Configuration;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration{
  public class FilterElementCollection:ConfigurationElementCollection{

    public FilterElementCollection(){
      FilterElement filter = (FilterElement)CreateNewElement();
      Add(filter);
    }

    public override ConfigurationElementCollectionType CollectionType{
      get{ return ConfigurationElementCollectionType.AddRemoveClearMap; }
    }

    protected override ConfigurationElement CreateNewElement(){
      return new FilterElement();
    }

    protected override object GetElementKey(ConfigurationElement element){
      return ((FilterElement)element).Name;
    }

    public FilterElement this[int index] {
      get {
        return (FilterElement)BaseGet(index);
      }
      set {
        if (BaseGet(index) != null)
        {
          BaseRemoveAt(index);
        }
        BaseAdd(index, value);
      }
    }

    new public FilterElement this[string Name] {
      get {
        return (FilterElement)BaseGet(Name);
      }
    }

    public int IndexOf(FilterElement url)
    {
      return BaseIndexOf(url);
    }

    public void Add(FilterElement url)
    {
      BaseAdd(url);
    }
    protected override void BaseAdd(ConfigurationElement element)
    {
      BaseAdd(element, false);
    }

    public void Remove(FilterElement url)
    {
      if (BaseIndexOf(url) >= 0)
        BaseRemove(url.Name);
    }

    public void RemoveAt(int index)
    {
      BaseRemoveAt(index);
    }

    public void Remove(string name)
    {
      BaseRemove(name);
    }

    public void Clear()
    {
      BaseClear();
    }


  }
}