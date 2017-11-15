using System.ComponentModel;
using System.Configuration;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Naming{
  internal class PublisherElement : ConfigurationElement{
    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    public string Name{
      get => (string) this["name"];
      set => this["name"] = value;
    }

    [ConfigurationProperty("action", DefaultValue = PublisherNamingAction.Remove)]
    [TypeConverter(typeof(CaseInsensitiveEnumConfigConverter<PublisherNamingAction>))]
    public PublisherNamingAction Action{ get; set; }
  }
}