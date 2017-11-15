namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Naming{
  internal class PublisherElementCollection : ElementCollectionBase<PublisherElement>{
    /// <inheritdoc />
    protected override object GetElementKey(PublisherElement element){
      return element.Name;
    }
  }

  internal enum PublisherNamingAction{
    Remove,
  }
}