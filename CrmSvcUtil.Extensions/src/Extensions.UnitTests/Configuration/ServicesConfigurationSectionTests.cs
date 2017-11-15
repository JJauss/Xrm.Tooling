using System;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Filter;
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
        Console.WriteLine("{0}", filter.Expression);
      }
    }

    //[TestMethod]
    //public void CreateWithoutConfiguration()
    //{
    //  using (ShimsContext.Create()){
    //    ShimConfigurationManager.GetSectionString = s => null;
    //    ServiceExtensionsConfigurationSection section = ServiceExtensionsConfigurationSection.Create();

    //    Assert.AreEqual("default", section.Filtering.EntityFilter[0].Name);
    //    Assert.AreEqual(".*", section.Filtering.EntityFilter[0].Expression);
    //    Assert.AreEqual(true, section.Filtering.EntityFilter[0].IgnoreCase);
    //  }
    //}
  }
}
