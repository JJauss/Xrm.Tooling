using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration;
using Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Naming;
using Beedev.Xrm.CrmSvcUtil.Extensions.Naming.Mapping;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Naming
{
  public class NamingService: INamingService
  {
    private readonly IDictionary<string, string> _args = new Dictionary<string, string>();
    private static readonly TraceSource ts = new TraceSource("Beedev.Xrm.CrmSvcUtil.Extensions", SourceLevels.Information);
    private readonly INamingService _defaultService;
    private readonly IServiceExtensionsConfiguration _configuration;
    private readonly IMapper _mapper;
    private Dictionary<OptionSetMetadataBase, Dictionary<string, int>> OptionNames{ get; }

    public NamingService(INamingService defaultService, IDictionary<string, string> args):this(defaultService, ServiceExtensionsConfigurationSection.Create()){
      _args = args;
    }

    internal NamingService(INamingService defaultService, IServiceExtensionsConfiguration configuration)
    {
      _defaultService = defaultService;
      _configuration = configuration;
      _mapper = new ConfigurationMapper(_configuration.Naming.Mapping);
      OptionNames = new Dictionary<OptionSetMetadataBase, Dictionary<string, int>>();
    }

    /// <inheritdoc />
    public string GetNameForOptionSet(EntityMetadata entityMetadata, OptionSetMetadataBase optionSetMetadata, IServiceProvider services){
      string value = _defaultService.GetNameForOptionSet(entityMetadata, optionSetMetadata, services);
      // For entity option set the name should be concatenated using the entity and attribute
      if (!optionSetMetadata.IsGlobal.GetValueOrDefault(false)){
        AttributeMetadata attribute = (from attr in entityMetadata.Attributes
          where attr.AttributeType == AttributeTypeCode.Picklist && ((EnumAttributeMetadata) attr).OptionSet.MetadataId == optionSetMetadata.MetadataId
          select attr).FirstOrDefault();

        if (attribute != null){
          value = string.Format(CultureInfo.InvariantCulture, "{0}{1}", GetNameForEntity(entityMetadata, services), GetNameForAttribute(entityMetadata, attribute, services));
        }
      }
      value = ModifyPublisher(value);
      return value;
    }

    /// <inheritdoc />
    public string GetNameForOption(OptionSetMetadataBase optionSetMetadata, OptionMetadata optionMetadata, IServiceProvider services){
      string value = _defaultService.GetNameForOption(optionSetMetadata, optionMetadata, services);

      value = EnsureValidIdentifier(value);
      value = EnsureUniqueOptionName(optionSetMetadata, value);
      value = ModifyPublisher(value);
      return value;
    }

    /// <inheritdoc />
    public string GetNameForEntity(EntityMetadata entityMetadata, IServiceProvider services){
      if (_mapper.GetNameFromMap(entityMetadata, services, out string value))
      {
        return value;
      }

      value =  _defaultService.GetNameForEntity(entityMetadata, services);
      value = ModifyPublisher(value);
      return value;
    }

    /// <inheritdoc />
    public string GetNameForAttribute(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata, IServiceProvider services){
      if (_mapper.GetNameFromMap(entityMetadata, attributeMetadata, services, out string value)){
        return value;
      }

      value = _defaultService.GetNameForAttribute(entityMetadata, attributeMetadata, services);
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
        if (name?.StartsWith(publisherElement.Name)?? false){
          name = name.Substring(publisherElement.Name.Length);
        }
      }


      return name;
    }

    /// <summary>
    /// Checks to make sure that the name begins with a valid character. If the name
    /// does not begin with a valid character, then add an underscore to the
    /// beginning of the name.
    /// </summary>
    private  string EnsureValidIdentifier(string name)
    {
      // Check to make sure that the option set begins with a word character
      // or underscore.
      var pattern = @"^[A-Za-z_][A-Za-z0-9_]*$";
      if (!Regex.IsMatch(name, pattern))
      {
        // Prepend an underscore to the name if it is not valid.
        name = $"_{name}";
        LogInformation($"Name of the option changed to {name}");
      }
      return name;
    }

    /// <summary>
    /// Checks to make sure that the name does not already exist for the OptionSet
    /// to be generated.
    /// </summary>
    private string EnsureUniqueOptionName(OptionSetMetadataBase metadata, string name)
    {
      if (OptionNames.ContainsKey(metadata))
      {
        if (OptionNames[metadata].ContainsKey(name))
        {
          // Increment the number of times that an option with this name has
          // been found.
          ++OptionNames[metadata][name];

          // Append the number to the name to create a new, unique name.
          var newName = $"{name}_{OptionNames[metadata][name]}";

          LogInformation($"The {metadata.Name} OptionSet already contained a definition for {name}. Changed to {newName}");

          // Call this function again to make sure that our new name is unique.
          return EnsureUniqueOptionName(metadata, newName);
        }
      }
      else
      {
        // This is the first time this OptionSet has been encountered. Add it to
        // the dictionary.
        OptionNames[metadata] = new Dictionary<string, int>();
      }

      // This is the first time this name has been encountered. Begin keeping track
      // of the times we've run across it.
      OptionNames[metadata][name] = 1;

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
