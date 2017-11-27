using System.Configuration;
using Microsoft.Xrm.Sdk;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Naming
{
  internal class NamingElement : ConfigurationElement
  {
    [ConfigurationProperty("Publisher")]
    [ConfigurationCollection(typeof(PublisherElementCollection))]
    public PublisherElementCollection Publisher {
      get => (PublisherElementCollection)base["Publisher"];
      set => base["Publisher"] = value;
    }

    [ConfigurationProperty("Mapping")]
    [ConfigurationCollection(typeof(MappingCollection))]
    public MappingCollection Mapping{
      get => (MappingCollection) base["Mapping"];
      set => base["Mapping"] = value;
    }
  }
}