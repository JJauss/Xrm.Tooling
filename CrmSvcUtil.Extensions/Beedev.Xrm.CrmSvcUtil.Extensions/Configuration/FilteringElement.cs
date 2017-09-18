using System.Configuration;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration{
  public class FilteringElement:ConfigurationElement{

    [ConfigurationProperty("EntityFilter")]
    [ConfigurationCollection(typeof(FilterElementCollection),
      AddItemName = "add",
      ClearItemsName = "clear",
      RemoveItemName = "remove")]
    public FilterElementCollection EntityFilter{
      get{ return (FilterElementCollection) base["EntityFilter"]; }
      set{ base["EntityFilter"] = value; }
    }

  }
}