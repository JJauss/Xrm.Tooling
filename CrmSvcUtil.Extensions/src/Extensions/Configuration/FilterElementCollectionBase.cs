using System.Configuration;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration{
  internal abstract class FilterElementCollectionBase<T>  : ConfigurationElementCollection
    where T : FilterElement, new()
  {
    protected FilterElementCollectionBase(){
      T filter = (T)CreateNewElement();
      Add(filter);
    }

    public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.AddRemoveClearMap;

    protected override ConfigurationElement CreateNewElement(){
      return new T();
    }

    protected override object GetElementKey(ConfigurationElement element){
      return ((T)element).Expression;
    }

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
        BaseRemove(element.Expression);
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