namespace Volo.Abp.BlobStoring.Gcp;

public class GcpBlobOptions
{
    public string ProjectId { get; set; }

    public string ClientEmail { get; set; }

    public string PrivateKey { get; set; }
}