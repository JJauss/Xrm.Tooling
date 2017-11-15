using System.Configuration;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Filter{
  internal class FilterElementCollection:ElementCollectionBase<FilterElement>{
    /// <inheritdoc />
    protected override object GetElementKey(FilterElement element){
      return element.Expression;
    }
  }

  internal class AttributeFilterElementCollection : ElementCollectionBase<AttributeFilterElement>
  {
    /// <inheritdoc />
    protected override object GetElementKey(AttributeFilterElement element)
    {
      return element.Expression;
    }
  }
}