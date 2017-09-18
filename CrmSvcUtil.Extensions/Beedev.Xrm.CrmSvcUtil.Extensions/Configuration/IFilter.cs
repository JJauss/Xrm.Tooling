namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration{
  public interface IFilter{
    string Name{ get; set; }
    string Expression{ get; set; }
    bool IgnoreCase{ get; set; }
  }
}