using System.Configuration;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration{
  public class AttributeFilterElement: FilterElement{

    public AttributeFilterElement(){
      
    }

    public AttributeFilterElement(string name, string entityName, string expression) : base(name, expression){
      this.EntityName = entityName;
    }

    [ConfigurationProperty("entity", IsRequired = true)]
    public string EntityName{
      get{ return (string) this["entity"]; }
      set{ this["entity"] = value; }
    }

  }
}