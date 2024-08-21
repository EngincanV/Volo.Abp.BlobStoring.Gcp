namespace Volo.Abp.BlobStoring.Gcp;

public static class GoogleCloudStorageBlobContainerConfigurationExtensions
{
    /// <summary>
    /// Uses Google Cloud Storage as blob provider.
    /// </summary>
    /// <returns></returns>
    public static BlobContainerConfiguration UseGoogleCloudStorage(this BlobContainerConfiguration configuration)
    {
        configuration.ProviderType = typeof(GoogleCloudStorageBlobProvider);

        return configuration;
    }
}