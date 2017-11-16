using Microsoft.VisualStudio.TestTools.UnitTesting;
using Beedev.Xrm.CrmSvcUtil.Extensions.Naming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Naming;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk.Metadata;
using Moq;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Naming.Tests
{
  [TestClass()]
  public class NamingServiceTests
  {
    private ServiceExtensionsConfigurationSection _configuration;
    private IServiceProvider _serviceProvider;
    private Mock<INamingService> _namingServiceMock;

    [TestInitialize]
    public void TestInitialize(){
      MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
      _configuration = new ServiceExtensionsConfigurationSection();
      _serviceProvider = mockRepository.Create<IServiceProvider>().Object;
      _namingServiceMock = mockRepository.Create<INamingService>();
      _namingServiceMock.Setup(mock => mock.GetNameForEntity(It.IsAny<EntityMetadata>(), _serviceProvider)).Returns<EntityMetadata, IServiceProvider>((metadata, services) => metadata.LogicalName);
    }

    [TestMethod()]
    public void GetNameForEntityTest(){
      NamingService namingService = new NamingService(_namingServiceMock.Object,_configuration);
      _configuration.Naming.Publisher.Add(new PublisherElement("beedev_"));
      Assert.AreEqual("account", namingService.GetNameForEntity(new EntityMetadata{LogicalName = "account"}, _serviceProvider));
      Assert.AreEqual("sampleEntity", namingService.GetNameForEntity(new EntityMetadata{LogicalName = "beedev_sampleEntity"}, _serviceProvider));
    }
  }
}