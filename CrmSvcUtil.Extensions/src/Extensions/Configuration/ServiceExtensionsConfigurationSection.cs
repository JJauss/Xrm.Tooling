using System.Configuration;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Filter;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Naming;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration
{
  internal interface IServiceExtensionsConfiguration{
    FilteringElement Filtering{ get; set; }
  }

  internal sealed class ServiceExtensionsConfigurationSection:ConfigurationSection, IServiceExtensionsConfiguration{
    private static string ServiceExtensionsSectionKey = "ServiceExtensions";

    public static ServiceExtensionsConfigurationSection Create(){
      if (!(ConfigurationManager.GetSection(ServiceExtensionsSectionKey) is ServiceExtensionsConfigurationSection section)){
        section = new ServiceExtensionsConfigurationSection();
      }
      return section;
    }

    [ConfigurationProperty("Filtering")]
    public FilteringElement Filtering{
      get => (FilteringElement) base["Filtering"];
      set => base["Filtering"] = value;
    }

    [ConfigurationProperty("Naming")]
    public NamingElement Naming{ get => (NamingElement) base["Naming"]; set => base["Naming"] = value; }
  }
}
