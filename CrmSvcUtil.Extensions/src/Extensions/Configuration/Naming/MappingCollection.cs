namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Naming{
  internal class MappingCollection : ElementCollectionBase<Map>
  {
    /// <inheritdoc />
    protected override object GetElementKey(Map element)
    {
      return element.From;
    }
  }
}