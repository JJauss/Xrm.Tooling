using System.Configuration;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Filter{
  internal class FilteringElement:ConfigurationElement{

    [ConfigurationProperty("EntityFilter")]
    [ConfigurationCollection(typeof(FilterElementCollection),
      AddItemName = "add",
      ClearItemsName = "clear",
      RemoveItemName = "remove")]
    public FilterElementCollection EntityFilter{
      get => (FilterElementCollection) base["EntityFilter"];
      set => base["EntityFilter"] = value;
    }

    [ConfigurationProperty("AttributeFilter")]
    [ConfigurationCollection(typeof(AttributeFilterElementCollection),
      AddItemName = "add",
      ClearItemsName = "clear",
      RemoveItemName = "remove")]
    public AttributeFilterElementCollection AttributeFilter {
      get => (AttributeFilterElementCollection)base["AttributeFilter"];
      set => base["AttributeFilter"] = value;
    }
  }
}