using System.Configuration;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration
{
  public interface IServiceExtensionsConfiguration{
    FilteringElement Filtering{ get; set; }
  }

  public sealed class ServiceExtensionsConfigurationSection:ConfigurationSection, IServiceExtensionsConfiguration{
    public static string ServiceExtensionsSectionKey = "ServiceExtensions";

    public static ServiceExtensionsConfigurationSection Create(){
      if (!(ConfigurationManager.GetSection(ServiceExtensionsSectionKey) is ServiceExtensionsConfigurationSection section)){
        // TODO: create default
        section = new ServiceExtensionsConfigurationSection();
      }
      return section;
    }

    [ConfigurationProperty("Filtering")]
    public FilteringElement Filtering{
      get{ return (FilteringElement) base["Filtering"]; } 
      set{ base["Filtering"] = value; }
    }
    

  }
}
