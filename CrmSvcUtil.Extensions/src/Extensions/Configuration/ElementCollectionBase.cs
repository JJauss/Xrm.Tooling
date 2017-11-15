using System.Configuration;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Filter;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration{
  internal abstract class ElementCollectionBase<T>  : ConfigurationElementCollection
    where T : ConfigurationElement,  new()
  {
    protected ElementCollectionBase(){
      T filter = (T)CreateNewElement();
      Add(filter);
    }

    public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.AddRemoveClearMap;

    protected override ConfigurationElement CreateNewElement(){
      return new T();
    }

    protected override object GetElementKey(ConfigurationElement element){
      return GetElementKey((T)element);
    }

    protected abstract object GetElementKey(T element);

    public T this[int index] {
      get => (T)BaseGet(index);
      set {
        if (BaseGet(index) != null)
        {
          BaseRemoveAt(index);
        }
        BaseAdd(index, value);
      }
    }

    public new T this[string name] => (T)BaseGet(name);

    public int IndexOf(T element)
    {
      return BaseIndexOf(element);
    }

    public void Add(T element)
    {
      BaseAdd(element);
    }

    protected override void BaseAdd(ConfigurationElement element)
    {
      BaseAdd(element, false);
    }

    public void Remove(T element)
    {
      if (BaseIndexOf(element) >= 0)
        BaseRemove(GetElementKey(element));
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