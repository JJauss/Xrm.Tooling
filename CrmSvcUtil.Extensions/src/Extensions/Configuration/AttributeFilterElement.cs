using System.Configuration;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration{
  public class AttributeFilterElement: FilterElement{

    public AttributeFilterElement(){
      
    }

    public AttributeFilterElement(string entityName, string expression) : base(expression){
      this.EntityName = entityName;
    }

    [ConfigurationProperty("entity", IsRequired = true)]
    public string EntityName{
      get{ return (string) this["entity"]; }
      set{ this["entity"] = value; }
    }

  }
}