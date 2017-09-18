using System;
using System.Fakes;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration;
using Microsoft.Crm.Services.Utility.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Metadata;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Filter{
  [TestClass]
  public class RegExFilterServiceTests{
    [TestMethod]
    public void GenerateEntityTest(){
      ServiceExtensionsConfigurationSection configuration = new ServiceExtensionsConfigurationSection();
      configuration.Filtering.EntityFilter.Clear();
      configuration.Filtering.EntityFilter.Add(new FilterElement("TestAccount", "^account$"));
      configuration.Filtering.EntityFilter.Add(new FilterElement("TestOpportunity", "^opportunity"));
      configuration.Filtering.EntityFilter.Add(new FilterElement("TestNew", "^new_.*$"));


      StubICodeWriterFilterService codeWriterFilterService = new StubICodeWriterFilterService{GenerateEntityEntityMetadataIServiceProvider = (metadata, provider) => true};
      RegExFilterService filterService = new RegExFilterService(codeWriterFilterService, configuration);

      IServiceProvider services = new StubIServiceProvider();
      Assert.IsTrue(filterService.GenerateEntity(new EntityMetadata{LogicalName = "account"}, services));
      Assert.IsTrue(filterService.GenerateEntity(new EntityMetadata{LogicalName = "opportunity"}, services));
      Assert.IsTrue(filterService.GenerateEntity(new EntityMetadata{LogicalName = "opportunity_detail"}, services));
      Assert.IsTrue(filterService.GenerateEntity(new EntityMetadata{LogicalName = "new_customentity"}, services));
    }

    [TestMethod]
    public void GenerateWithInvalidExpressionTest(){
      ServiceExtensionsConfigurationSection configuration = new ServiceExtensionsConfigurationSection();
      configuration.Filtering.EntityFilter.Clear();
      configuration.Filtering.EntityFilter.Add(new FilterElement("TestAccount", "^account$"));
      configuration.Filtering.EntityFilter.Add(new FilterElement("TestOpportunity", "^\\opportunity"));
      configuration.Filtering.EntityFilter.Add(new FilterElement("TestNew", "^new_.*$"));


      StubICodeWriterFilterService codeWriterFilterService = new StubICodeWriterFilterService{GenerateEntityEntityMetadataIServiceProvider = (metadata, provider) => true};
      RegExFilterService filterService = new RegExFilterService(codeWriterFilterService, configuration);

      IServiceProvider services = new StubIServiceProvider();
      Assert.IsTrue(filterService.GenerateEntity(new EntityMetadata{LogicalName = "account"}, services));
      Assert.IsFalse(filterService.GenerateEntity(new EntityMetadata{LogicalName = "opportunity"}, services));
      Assert.IsFalse(filterService.GenerateEntity(new EntityMetadata{LogicalName = "opportunity_detail"}, services));
      Assert.IsTrue(filterService.GenerateEntity(new EntityMetadata{LogicalName = "new_customentity"}, services));
    }

    [TestMethod]
    public void GenerateWithoutConfigurationTest()
    {
      ServiceExtensionsConfigurationSection configuration = new ServiceExtensionsConfigurationSection();
      configuration.Filtering.EntityFilter.Clear();

      StubICodeWriterFilterService codeWriterFilterService = new StubICodeWriterFilterService { GenerateEntityEntityMetadataIServiceProvider = (metadata, provider) => true };
      RegExFilterService filterService = new RegExFilterService(codeWriterFilterService, configuration);

      IServiceProvider services = new StubIServiceProvider();
      Assert.IsTrue(filterService.GenerateEntity(new EntityMetadata { LogicalName = "account" }, services));
      Assert.IsTrue(filterService.GenerateEntity(new EntityMetadata { LogicalName = "opportunity" }, services));
      Assert.IsTrue(filterService.GenerateEntity(new EntityMetadata { LogicalName = "opportunity_detail" }, services));
      Assert.IsTrue(filterService.GenerateEntity(new EntityMetadata { LogicalName = "new_customentity" }, services));
    }
  }
}