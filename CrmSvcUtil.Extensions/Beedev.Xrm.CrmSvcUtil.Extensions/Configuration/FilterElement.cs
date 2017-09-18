using System.Configuration;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration{
  public class FilterElement:ConfigurationElement, IFilter{
    public FilterElement(){
      Name = "default";
      Expression = ".*";
    }
    public FilterElement(string name, string expression){
      Name = name;
      Expression = expression;
    }

    

    [ConfigurationProperty("name", DefaultValue = "default", IsKey = true, IsRequired = true)]
    public string Name{
      get{ return (string) this["name"]; }
      set{ this["name"] = value; }
    }
    

    [ConfigurationProperty("expression", IsRequired = true)]
    public string Expression {
      get { return (string)this["expression"]; }
      set { this["expression"] = value; }
    }

    [ConfigurationProperty("ignoreCase", DefaultValue = true)]
    public bool IgnoreCase{
      get{ return (bool) this["ignoreCase"]; }
      set{ this["ignoreCase"] = value; }
    }
  }
}