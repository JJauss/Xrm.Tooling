using System;
using System.Diagnostics;
using System.Linq;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Naming;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Naming
{
  public class NamingService: INamingService
  {
    private static readonly TraceSource ts = new TraceSource("Beedev.Xrm.CrmSvcUtil.Extensions", SourceLevels.Information);
    private readonly INamingService _defaultService;
    private readonly IServiceExtensionsConfiguration _configuration;

    public NamingService(INamingService defaultService):this(defaultService, ServiceExtensionsConfigurationSection.Create())
    {
    }

    internal NamingService(INamingService defaultService, IServiceExtensionsConfiguration configuration)
    {
      _defaultService = defaultService;
      _configuration = configuration;
    }

    /// <inheritdoc />
    public string GetNameForOptionSet(EntityMetadata entityMetadata, OptionSetMetadataBase optionSetMetadata, IServiceProvider services){
      string value = _defaultService.GetNameForOptionSet(entityMetadata, optionSetMetadata, services);
      value = ModifyPublisher(value);
      return value;
    }

    /// <inheritdoc />
    public string GetNameForOption(OptionSetMetadataBase optionSetMetadata, OptionMetadata optionMetadata, IServiceProvider services){
      string value = _defaultService.GetNameForOption(optionSetMetadata, optionMetadata, services);
      value = ModifyPublisher(value);
      return value;
    }

    /// <inheritdoc />
    public string GetNameForEntity(EntityMetadata entityMetadata, IServiceProvider services){
      string value =  _defaultService.GetNameForEntity(entityMetadata, services);
      value = ModifyPublisher(value);
      return value;
    }

    /// <inheritdoc />
    public string GetNameForAttribute(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata, IServiceProvider services){
      string value = _defaultService.GetNameForAttribute(entityMetadata, attributeMetadata, services);
      value = ModifyPublisher(value);
      return value;
    }

    /// <inheritdoc />
    public string GetNameForRelationship(EntityMetadata entityMetadata, RelationshipMetadataBase relationshipMetadata, EntityRole? reflexiveRole, IServiceProvider services){
      string value = _defaultService.GetNameForRelationship(entityMetadata, relationshipMetadata, reflexiveRole, services);
      value = ModifyPublisher(value);
      return value;
    }

    /// <inheritdoc />
    public string GetNameForServiceContext(IServiceProvider services){
      string value = _defaultService.GetNameForServiceContext(services);
      value = ModifyPublisher(value);
      return value;
    }

    /// <inheritdoc />
    public string GetNameForEntitySet(EntityMetadata entityMetadata, IServiceProvider services){
      string value = _defaultService.GetNameForEntitySet(entityMetadata, services);
      value = ModifyPublisher(value);
      return value;
    }

    /// <inheritdoc />
    public string GetNameForMessagePair(SdkMessagePair messagePair, IServiceProvider services){
      string value = _defaultService.GetNameForMessagePair(messagePair, services);
      value = ModifyPublisher(value);
      return value;
    }

    /// <inheritdoc />
    public string GetNameForRequestField(SdkMessageRequest request, SdkMessageRequestField requestField, IServiceProvider services){
      string value = _defaultService.GetNameForRequestField(request, requestField, services);
      value = ModifyPublisher(value);
      return value;
    }

    /// <inheritdoc />
    public string GetNameForResponseField(SdkMessageResponse response, SdkMessageResponseField responseField, IServiceProvider services){
      string value = _defaultService.GetNameForResponseField(response, responseField, services);
      value = ModifyPublisher(value);
      return value;
    }

    private string ModifyPublisher(string name){
      foreach (PublisherElement publisherElement in _configuration.Naming.Publisher.Where(p => p.Action == PublisherNamingAction.Remove)){
        if (name.StartsWith(publisherElement.Name)){
          name = name.Substring(publisherElement.Name.Length);
        }
      }


      return name;
    }

    private void LogInformation(string msg)
    {
      ts.TraceEvent(TraceEventType.Information, 100, $"{GetType().Name}: {msg}");
    }

    private void LogInformation(string msg, params object[] args)
    {
      ts.TraceEvent(TraceEventType.Information, 100, $"{GetType().Name}: {msg}", args);
    }

    private void LogExceptionWarning(Exception ex)
    {
      ts.TraceEvent(TraceEventType.Warning, 100, ex.ToString());
    }
  }
}
