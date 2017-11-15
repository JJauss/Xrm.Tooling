using System.Configuration;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Naming{
  internal class NamingElement : ConfigurationElement{
    [ConfigurationProperty("Publisher")]
    [ConfigurationCollection(typeof(PublisherElementCollection))]
    public PublisherElementCollection Publisher{
      get => (PublisherElementCollection) base["Publisher"];
      set => base["Publisher"] = value;
    }
  }
}