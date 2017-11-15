namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration.Filter{
  public interface IFilter{
    string Expression{ get; set; }
    bool IgnoreCase{ get; set; }
  }
}