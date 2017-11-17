using System;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Moq;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Mocks{
  internal class NamingServiceMock: INamingService{
    private readonly Mock<INamingService> _mock;
    private INamingService NamingServiceImplementation => _mock?.Object;

    public static NamingServiceMock Create(MockRepository repository, string serviceContextName, IServiceProvider serviceProvider){
      Mock<INamingService> namingServiceMock = repository.Create<INamingService>();
      namingServiceMock.Setup(mock => mock.GetNameForEntity(It.IsAny<EntityMetadata>(), serviceProvider)).Returns<EntityMetadata, IServiceProvider>((entity, services) => entity.SchemaName);
      namingServiceMock.Setup(mock => mock.GetNameForEntitySet(It.IsAny<EntityMetadata>(), serviceProvider)).Returns<EntityMetadata, IServiceProvider>((entity, services) => entity.CollectionSchemaName);
      namingServiceMock.Setup(mock => mock.GetNameForAttribute(It.IsAny<EntityMetadata>(), It.IsAny<AttributeMetadata>(), serviceProvider)).Returns<EntityMetadata, AttributeMetadata, IServiceProvider>((entity, attribute, services) => attribute.SchemaName);
      namingServiceMock.Setup(mock => mock.GetNameForRelationship(It.IsAny<EntityMetadata>(), It.IsAny<RelationshipMetadataBase>(), null, serviceProvider)).Returns<EntityMetadata, RelationshipMetadataBase, EntityRole?, IServiceProvider>((entity, relation, role, services) => relation.SchemaName);
      namingServiceMock.Setup(mock => mock.GetNameForOptionSet(It.IsAny<EntityMetadata>(), It.IsAny<OptionSetMetadata>(), serviceProvider)).Returns<EntityMetadata, OptionSetMetadata, IServiceProvider>((entity, optionSet, services) => optionSet.Name);
      namingServiceMock.Setup(mock => mock.GetNameForOption(It.IsAny<OptionSetMetadata>(), It.IsAny<OptionMetadata>(), serviceProvider)).Returns<OptionSetMetadata, OptionMetadata, IServiceProvider>((optionSet, option, services) => option?.Label?.UserLocalizedLabel?.Label);
      namingServiceMock.Setup(mock => mock.GetNameForServiceContext(serviceProvider)).Returns<IServiceProvider>((services) => serviceContextName);
      namingServiceMock.Setup(mock => mock.GetNameForMessagePair(It.IsAny<SdkMessagePair>(), serviceProvider)).Returns<SdkMessagePair, IServiceProvider>((messagePair, services) => messagePair.Message.Name);
      namingServiceMock.Setup(mock => mock.GetNameForRequestField(It.IsAny<SdkMessageRequest>(), It.IsAny<SdkMessageRequestField>(), serviceProvider)).Returns<SdkMessageRequest, SdkMessageRequestField, IServiceProvider>((request, field, services) => field.Name);
      namingServiceMock.Setup(mock => mock.GetNameForResponseField(It.IsAny<SdkMessageResponse>(), It.IsAny<SdkMessageResponseField>(), serviceProvider)).Returns<SdkMessageResponse, SdkMessageResponseField, IServiceProvider>((response, field, services) => field.Name);
      return new NamingServiceMock(namingServiceMock);
    }

    private NamingServiceMock(Mock<INamingService> namingServiceMock){
      _mock = namingServiceMock;
    }


    public Mock<INamingService> InternalMock => _mock;
    /// <inheritdoc />
    public string GetNameForOptionSet(EntityMetadata entityMetadata, OptionSetMetadataBase optionSetMetadata, IServiceProvider services){
      return NamingServiceImplementation.GetNameForOptionSet(entityMetadata, optionSetMetadata, services);
    }

    /// <inheritdoc />
    public string GetNameForOption(OptionSetMetadataBase optionSetMetadata, OptionMetadata optionMetadata, IServiceProvider services){
      return NamingServiceImplementation.GetNameForOption(optionSetMetadata, optionMetadata, services);
    }

    /// <inheritdoc />
    public string GetNameForEntity(EntityMetadata entityMetadata, IServiceProvider services){
      return NamingServiceImplementation.GetNameForEntity(entityMetadata, services);
    }

    /// <inheritdoc />
    public string GetNameForAttribute(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata, IServiceProvider services){
      return NamingServiceImplementation.GetNameForAttribute(entityMetadata, attributeMetadata, services);
    }

    /// <inheritdoc />
    public string GetNameForRelationship(EntityMetadata entityMetadata, RelationshipMetadataBase relationshipMetadata, EntityRole? reflexiveRole, IServiceProvider services){
      return NamingServiceImplementation.GetNameForRelationship(entityMetadata, relationshipMetadata, reflexiveRole, services);
    }

    /// <inheritdoc />
    public string GetNameForServiceContext(IServiceProvider services){
      return NamingServiceImplementation.GetNameForServiceContext(services);
    }

    /// <inheritdoc />
    public string GetNameForEntitySet(EntityMetadata entityMetadata, IServiceProvider services){
      return NamingServiceImplementation.GetNameForEntitySet(entityMetadata, services);
    }

    /// <inheritdoc />
    public string GetNameForMessagePair(SdkMessagePair messagePair, IServiceProvider services){
      return NamingServiceImplementation.GetNameForMessagePair(messagePair, services);
    }

    /// <inheritdoc />
    public string GetNameForRequestField(SdkMessageRequest request, SdkMessageRequestField requestField, IServiceProvider services){
      return NamingServiceImplementation.GetNameForRequestField(request, requestField, services);
    }

    /// <inheritdoc />
    public string GetNameForResponseField(SdkMessageResponse response, SdkMessageResponseField responseField, IServiceProvider services){
      return NamingServiceImplementation.GetNameForResponseField(response, responseField, services);
    }
  }
}