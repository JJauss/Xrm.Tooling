using System;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration;
using Microsoft.Crm.Services.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Metadata;
using Moq;
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
      _configuration.Filtering.EntityFilter.Add(new FilterElement("^account$"));
      _configuration.Filtering.EntityFilter.Add(new FilterElement("^opportunity"));
      _configuration.Filtering.EntityFilter.Add(new FilterElement("^new_.*$"));
      _configuration.Filtering.AttributeFilter.Clear();
      _configuration.Filtering.AttributeFilter.Add(new AttributeFilterElement("account", "^name$"));
      _configuration.Filtering.AttributeFilter.Add(new AttributeFilterElement("opportunity", "^title"));
      _configuration.Filtering.AttributeFilter.Add(new AttributeFilterElement("new_customentity", "^new_.*$"));

      _accountNameMetadata = JObject.Parse("{logicalName:\"name\", entityLogicalName:\"account\"}").ToObject<AttributeMetadata>();
      _opportunityTitleMetadata = JObject.Parse("{logicalName:\"title\", entityLogicalName:\"opportunity\"}").ToObject<AttributeMetadata>();
      _opportunityNameMetadata = JObject.Parse("{logicalName:\"name\", entityLogicalName:\"opportunity\"}").ToObject<AttributeMetadata>();

      MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
      Mock<ICodeWriterFilterService> codeWriterFilterServiceMock = mockRepository.Create<ICodeWriterFilterService>();
      codeWriterFilterServiceMock.Setup(_ => _.GenerateEntity(It.IsAny<EntityMetadata>(), It.IsAny<IServiceProvider>()))
                                 .Returns(true);
      codeWriterFilterServiceMock.Setup(_ => _.GenerateAttribute(It.IsAny<AttributeMetadata>(), It.IsAny<IServiceProvider>()))
                                 .Returns(true);
      _regExFilterService = new RegExFilterService(codeWriterFilterServiceMock.Object, _configuration);
      _serviceProvider = mockRepository.Create<IServiceProvider>().Object;
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
      _configuration.Filtering.EntityFilter.Remove("^opportunity");
      _configuration.Filtering.EntityFilter.Add(new FilterElement("^\\opportunity"));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata{LogicalName = "account"}, _serviceProvider));
      Assert.IsFalse(_regExFilterService.GenerateEntity(new EntityMetadata{LogicalName = "opportunity"}, _serviceProvider));
      Assert.IsFalse(_regExFilterService.GenerateEntity(new EntityMetadata{LogicalName = "opportunity_detail"}, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata{LogicalName = "new_customentity"}, _serviceProvider));
    }

    [TestMethod]
    public void GenerateWithoutConfigurationTest()
    {
      _configuration.Filtering.EntityFilter.Clear();
      _configuration.Filtering.AttributeFilter.Clear();
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata { LogicalName = "account" }, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata { LogicalName = "opportunity" }, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata { LogicalName = "contact" }, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata { LogicalName = "opportunity_detail" }, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateEntity(new EntityMetadata { LogicalName = "new_customentity" }, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateAttribute(_accountNameMetadata, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateAttribute(_opportunityTitleMetadata, _serviceProvider));
    }

    [TestMethod]
    public void GenerateAttributesForUnconfiguredEntityTest()
    {
      _configuration.Filtering.EntityFilter.Clear();
      _configuration.Filtering.AttributeFilter.Clear();
      _configuration.Filtering.AttributeFilter.Add(new AttributeFilterElement("account", "^(?!name)$"));
      Assert.IsFalse(_regExFilterService.GenerateAttribute(_accountNameMetadata, _serviceProvider));
      Assert.IsTrue(_regExFilterService.GenerateAttribute(_opportunityTitleMetadata, _serviceProvider));
    }
  }
}