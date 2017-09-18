using System;
using System.Fakes;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration;
using Microsoft.Crm.Services.Utility.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Metadata;
using Newtonsoft.Json.Linq;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Filter{
  [TestClass]
  public class RegExFilterServiceTests{
    private ServiceExtensionsConfigurationSection _configuration;
    private AttributeMetadata _accountNameMetadata;
    private AttributeMetadata _opportunityTitleMetadata;
    private AttributeMetadata _opportunityNameMetadata;
    private RegExFilterService _regExFilterService;
    private IServiceProvider _serviceProvider;

    [TestInitialize]
    public void TestInitialize(){
      _configuration = new ServiceExtensionsConfigurationSection();
      _configuration.Filtering.EntityFilter.Clear();
      _configuration.Filtering.EntityFilter.Add(new FilterElement("TestAccount", "^account$"));
      _configuration.Filtering.EntityFilter.Add(new FilterElement("TestOpportunity", "^opportunity"));
      _configuration.Filtering.EntityFilter.Add(new FilterElement("TestNew", "^new_.*$"));
      _configuration.Filtering.AttributeFilter.Clear();
      _configuration.Filtering.AttributeFilter.Add(new AttributeFilterElement("TestAccount", "account", "^name$"));
      _configuration.Filtering.AttributeFilter.Add(new AttributeFilterElement("TestOpportunity", "opportunity", "^title"));
      _configuration.Filtering.AttributeFilter.Add(new AttributeFilterElement("NewCustom", "new_customentity", "^new_.*$"));

      _accountNameMetadata = JObject.Parse("{logicalName:\"name\", entityLogicalName:\"account\"}").ToObject<AttributeMetadata>();
      _opportunityTitleMetadata = JObject.Parse("{logicalName:\"title\", entityLogicalName:\"opportunity\"}").ToObject<AttributeMetadata>();
      _opportunityNameMetadata = JObject.Parse("{logicalName:\"name\", entityLogicalName:\"opportunity\"}").ToObject<AttributeMetadata>();


      StubICodeWriterFilterService codeWriterFilterService = new StubICodeWriterFilterService
      {
        GenerateEntityEntityMetadataIServiceProvider = (metadata, provider) => true,
        GenerateAttributeAttributeMetadataIServiceProvider = (metadata, provider) => true
      };
      _regExFilterService = new RegExFilterService(codeWriterFilterService, _configuration);
      _serviceProvider = new StubIServiceProvider();
    }

    [TestMethod]
    public void GenerateEntityTest(){
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata{LogicalName = "account"}, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata{LogicalName = "opportunity"}, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata{LogicalName = "opportunity_detail"}, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata{LogicalName = "new_customentity"}, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateAttribute(_accountNameMetadata, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateAttribute(_opportunityTitleMetadata, _serviceProvider));
      Assert.IsFalse(_regExFilterService.GenerateAttribute(_opportunityNameMetadata, _serviceProvider));
    }

    [TestMethod]
    public void GenerateWithInvalidExpressionTest(){
      _configuration.Filtering.EntityFilter.Add(new FilterElement("TestOpportunity", "^\\opportunity"));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata{LogicalName = "account"}, _serviceProvider));
      Assert.IsFalse(_regExFilterService.GenerateEntity(new EntityMetadata{LogicalName = "opportunity"}, _serviceProvider));
      Assert.IsFalse(_regExFilterService.GenerateEntity(new EntityMetadata{LogicalName = "opportunity_detail"}, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata{LogicalName = "new_customentity"}, _serviceProvider));
    }

    [TestMethod]
    public void GenerateWithoutConfigurationTest()
    {
      _configuration.Filtering.EntityFilter.Clear();
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata { LogicalName = "account" }, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata { LogicalName = "opportunity" }, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata { LogicalName = "contact" }, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata { LogicalName = "opportunity_detail" }, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata { LogicalName = "new_customentity" }, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateAttribute(_accountNameMetadata, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateAttribute(_opportunityTitleMetadata, _serviceProvider));
    }
  }
}