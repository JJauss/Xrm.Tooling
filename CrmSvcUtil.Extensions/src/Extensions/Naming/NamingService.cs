using System;
using System.Diagnostics;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Naming
{
  public class NamingService: INamingService
  {
    private static readonly TraceSource ts = new TraceSource("Beedev.Xrm.CrmSvcUtil.Extensions", SourceLevels.Information);
    private INamingService _defaultService;
    private IServiceExtensionsConfiguration _configuration;

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
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string GetNameForOption(OptionSetMetadataBase optionSetMetadata, OptionMetadata optionMetadata, IServiceProvider services){
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string GetNameForEntity(EntityMetadata entityMetadata, IServiceProvider services){
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string GetNameForAttribute(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata, IServiceProvider services){
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string GetNameForRelationship(EntityMetadata entityMetadata, RelationshipMetadataBase relationshipMetadata, EntityRole? reflexiveRole, IServiceProvider services){
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string GetNameForServiceContext(IServiceProvider services){
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string GetNameForEntitySet(EntityMetadata entityMetadata, IServiceProvider services){
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string GetNameForMessagePair(SdkMessagePair messagePair, IServiceProvider services){
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string GetNameForRequestField(SdkMessageRequest request, SdkMessageRequestField requestField, IServiceProvider services){
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string GetNameForResponseField(SdkMessageResponse response, SdkMessageResponseField responseField, IServiceProvider services){
      throw new NotImplementedException();
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
