namespace Epay3.Api.Models.Integration
{
    public class SyncCardRequest
    {
        public string TargetClientCode { get; set; }
        public string TargetClientToken { get; set; }
        public bool SyncPassiveCards { get; set; }
    }
}