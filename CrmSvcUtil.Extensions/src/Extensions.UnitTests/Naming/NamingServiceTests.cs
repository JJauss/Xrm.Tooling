using System;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Naming;
using Beedev.Xrm.CrmSvcUtil.Extensions.Naming.Tests;
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
    private DefaultNamingServiceMock _namingServiceMock;

    [TestInitialize]
    public void TestInitialize(){
      MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
      _configuration = new ServiceExtensionsConfigurationSection();
      _serviceProvider = mockRepository.Create<IServiceProvider>().Object;
      _namingServiceMock = DefaultNamingServiceMock.Create(mockRepository, _serviceProvider);
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
    }
  }
}