using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration
{
  [TestClass]
  public class ServicesConfigurationSectionTests
  {
    [TestMethod]
    public void CreateConfiguration(){
      ServiceExtensionsConfigurationSection section = ServiceExtensionsConfigurationSection.Create();

      foreach (FilterElement filter in section.Filtering.EntityFilter){
        Console.WriteLine("{0} - {1}", filter.Name, filter.Expression);
      }
    }
  }
}
