namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration{
  public interface IFilter{
    string Expression{ get; set; }
    bool IgnoreCase{ get; set; }
  }
}