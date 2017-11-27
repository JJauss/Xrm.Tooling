using System;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Naming;
using Beedev.Xrm.CrmSvcUtil.Extensions.Mocks;
using Microsoft.Crm.Services.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Moq;
using Newtonsoft.Json;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Naming
{
  [TestClass]
  public class NamingServiceTests
  {
    private ServiceExtensionsConfigurationSection _configuration;
    private IServiceProvider _serviceProvider;
    private NamingServiceMock _namingServiceMock;

    [TestInitialize]
    public void TestInitialize(){
      MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
      _configuration = new ServiceExtensionsConfigurationSection();
      _serviceProvider = mockRepository.Create<IServiceProvider>().Object;
      _namingServiceMock = NamingServiceMock.Create(mockRepository,"ServiceContextName", _serviceProvider);
    }

    [TestMethod]
    public void GetNameForEntityTest(){
      NamingService namingService = new NamingService(_namingServiceMock,_configuration);
      _configuration.Naming.Publisher.Add(new PublisherElement("beedev_"));
      EntityMetadata acountMetadata = JsonConvert.DeserializeObject<EntityMetadata>("{\"SchemaName\":\"Account\", \"CollectionSchemaName\":\"AccountSet\"}");
      EntityMetadata sampleEntityMetadata = JsonConvert.DeserializeObject<EntityMetadata>("{\"SchemaName\":\"beedev_SampleEntity\", \"CollectionSchemaName\":\"beedev_SampleEntitySet\"}");

      Assert.AreEqual("Account", namingService.GetNameForEntity(acountMetadata, _serviceProvider));
      Assert.AreEqual("SampleEntity", namingService.GetNameForEntity(sampleEntityMetadata, _serviceProvider));

      Assert.AreEqual("AccountSet", namingService.GetNameForEntitySet(acountMetadata, _serviceProvider));
      Assert.AreEqual("SampleEntitySet", namingService.GetNameForEntitySet(sampleEntityMetadata, _serviceProvider));

      Assert.AreEqual("Name", namingService.GetNameForAttribute(null, new AttributeMetadata{SchemaName = "Name"}, _serviceProvider));
      Assert.AreEqual("Name", namingService.GetNameForAttribute(null, new AttributeMetadata{SchemaName = "beedev_Name"}, _serviceProvider));

      Assert.AreEqual("Name", namingService.GetNameForOptionSet(acountMetadata, new OptionSetMetadata{Name = "Name"}, _serviceProvider));
      Assert.AreEqual("Name", namingService.GetNameForOptionSet(acountMetadata, new OptionSetMetadata{Name = "beedev_Name"}, _serviceProvider));

      Assert.AreEqual("Name", namingService.GetNameForOption(new OptionSetMetadata(), new OptionMetadata{Label = new Label("Name", 1033){UserLocalizedLabel = new LocalizedLabel("Name", 1033)}},  _serviceProvider));
      Assert.AreEqual("Name", namingService.GetNameForOption(new OptionSetMetadata(), new OptionMetadata{Label = new Label("beedev_Name", 1033){UserLocalizedLabel = new LocalizedLabel("beedev_Name", 1033)}},  _serviceProvider));

      Assert.AreEqual("Account_Opportunities", namingService.GetNameForRelationship(acountMetadata, new ManyToManyRelationshipMetadata{SchemaName = "Account_Opportunities"}, new EntityRole?(), _serviceProvider));
      Assert.AreEqual("SampleEntity_Opportunities", namingService.GetNameForRelationship(acountMetadata, new ManyToManyRelationshipMetadata{SchemaName = "beedev_SampleEntity_Opportunities"}, new EntityRole?(), _serviceProvider));

      Assert.AreEqual("ServiceContextName", namingService.GetNameForServiceContext(_serviceProvider));

      SdkMessagePair createMessagePair = new SdkMessagePair(new SdkMessage(Guid.NewGuid(), "Create", false), Guid.NewGuid(), "" );
      SdkMessagePair beedevCreateMessage = new SdkMessagePair(new SdkMessage(Guid.NewGuid(), "beedev_Create", false), Guid.NewGuid(), "" );
      Assert.AreEqual("Create", namingService.GetNameForMessagePair(createMessagePair, _serviceProvider));
      Assert.AreEqual("Create", namingService.GetNameForMessagePair(beedevCreateMessage, _serviceProvider));

      Assert.AreEqual("RequestField", namingService.GetNameForRequestField(new SdkMessageRequest(createMessagePair,Guid.NewGuid(), "CreateRequest"), new SdkMessageRequestField(new SdkMessageRequest(createMessagePair, Guid.NewGuid(), "CreateRequest"),0, "RequestField", "", true ), _serviceProvider));
      Assert.AreEqual("RequestField", namingService.GetNameForRequestField(new SdkMessageRequest(createMessagePair,Guid.NewGuid(), "CreateRequest"), new SdkMessageRequestField(new SdkMessageRequest(createMessagePair, Guid.NewGuid(), "CreateRequest"),0, "beedev_RequestField", "", true ), _serviceProvider));

      Assert.AreEqual("ResponseField", namingService.GetNameForResponseField(new SdkMessageResponse(Guid.NewGuid()), new SdkMessageResponseField(0, "ResponseField", "", null), _serviceProvider));
      Assert.AreEqual("ResponseField", namingService.GetNameForResponseField(new SdkMessageResponse(Guid.NewGuid()), new SdkMessageResponseField(0, "beedev_ResponseField", "", null), _serviceProvider));
    }

    [TestMethod]
    public void MapNameTest(){
      NamingService namingService = new NamingService(_namingServiceMock, _configuration);
      _configuration.Naming.Mapping.Clear();
      _configuration.Naming.Mapping.Add(new Map("sample_15entity", "Entity15"));
      _configuration.Naming.Mapping.Add(new Map("sample_15entity.01_address", "MyAddress"){Type = MapType.Attribute});

      EntityMetadata entityMetadata = new EntityMetadata{LogicalName = "sample_15entity"};
      AttributeMetadata attributeMetadata = JsonConvert.DeserializeObject<AttributeMetadata>("{\"LogicalName\":\"01_address\", \"EntityLogicalName\":\"sample_15entity\"}");
      Assert.AreEqual("Entity15", namingService.GetNameForEntity(entityMetadata,_serviceProvider));
      Assert.AreEqual("MyAddress", namingService.GetNameForAttribute(entityMetadata,attributeMetadata,  _serviceProvider));
    }

  }
}