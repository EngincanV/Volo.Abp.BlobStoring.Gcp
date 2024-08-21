using Google.Apis.Storage.v1;

namespace Volo.Abp.BlobStoring.Gcp;

public class GoogleCloudStorageBlobOptions
{
    public string ProjectId { get; set; }

    public string ClientEmail { get; set; }

    public string PrivateKey { get; set; }

    public List<string> Scopes { get; set; } = new()
    {
        StorageService.Scope.DevstorageFullControl
    };
}