using System.Configuration;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Filter{
  public class FilterElement:ConfigurationElement, IFilter{
    public FilterElement(){
      Expression = ".*";
    }
    public FilterElement(string expression){
      Expression = expression;
    }
    
    [ConfigurationProperty("expression", IsRequired = true)]
    public string Expression {
      get => (string)this["expression"];
      set => this["expression"] = value;
    }

    [ConfigurationProperty("ignoreCase", DefaultValue = true)]
    public bool IgnoreCase{
      get => (bool) this["ignoreCase"];
      set => this["ignoreCase"] = value;
    }
  }
}