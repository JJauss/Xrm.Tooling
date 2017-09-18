using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk.Metadata;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Filter{
  public class RegExFilterService : ICodeWriterFilterService{
    private static readonly TraceSource ts = new TraceSource("Beedev.Xrm.CrmSvcUtil.Extensions", SourceLevels.Information);
    private readonly ICodeWriterFilterService _defaultService;
    private readonly IServiceExtensionsConfiguration _configuration;

    public RegExFilterService(ICodeWriterFilterService defaultService):this(defaultService, ServiceExtensionsConfigurationSection.Create())
    {
    }

    internal RegExFilterService(ICodeWriterFilterService defaultService, IServiceExtensionsConfiguration configuration){
      _defaultService = defaultService;
      _configuration = configuration;
    }

    public bool GenerateOptionSet(OptionSetMetadataBase optionSetMetadata, IServiceProvider services){
      return _defaultService.GenerateOptionSet(optionSetMetadata, services);
    }

    public bool GenerateOption(OptionMetadata optionMetadata, IServiceProvider services){
      return _defaultService.GenerateOption(optionMetadata, services);
    }

    public bool GenerateEntity(EntityMetadata entityMetadata, IServiceProvider services){
      bool generate = _defaultService.GenerateEntity(entityMetadata, services);

      if (generate){
        generate = DoesMatchSettings(entityMetadata);
      }
      return generate;
    }

    private bool DoesMatchSettings(EntityMetadata entityMetadata){
      bool result = _configuration.Filtering.EntityFilter.Count == 0;
      for (int index = 0; index < _configuration.Filtering.EntityFilter.Count && !result; index++){
        FilterElement filterElement = _configuration.Filtering.EntityFilter[index];
        LogInformation($"Match {filterElement.Name} ({filterElement.Expression}) with entity logical name: {entityMetadata.LogicalName}");
        try{
          RegexOptions options = filterElement.IgnoreCase ? RegexOptions.IgnoreCase : ~ RegexOptions.IgnoreCase;
          Regex regex = new Regex(filterElement.Expression, options);
          bool isMatch = regex.IsMatch(entityMetadata.LogicalName);
          LogInformation("{0} {1}",filterElement.Name , isMatch?"matches":"does not match");
          result |= isMatch;
        }
        catch (ArgumentException ex){
          LogExceptionWarning(ex);
        }
      }
      return result;
    }

    public bool GenerateAttribute(AttributeMetadata attributeMetadata, IServiceProvider services){
      return _defaultService.GenerateAttribute(attributeMetadata, services);
    }

    public bool GenerateRelationship(RelationshipMetadataBase relationshipMetadata, EntityMetadata otherEntityMetadata, IServiceProvider services){
      return _defaultService.GenerateRelationship(relationshipMetadata, otherEntityMetadata, services);
    }

    public bool GenerateServiceContext(IServiceProvider services){
      return _defaultService.GenerateServiceContext(services);
    }

    private void LogInformation(string msg){
      ts.TraceEvent(TraceEventType.Information, 100, $"{GetType().Name}: {msg}");
    }
    private void LogInformation(string msg, params object[] args){
      ts.TraceEvent(TraceEventType.Information, 100, $"{GetType().Name}: {msg}", args);
    }

    private void LogExceptionWarning(Exception ex)
    {
      ts.TraceEvent(TraceEventType.Warning, 100, ex.ToString());
    }
  }
}