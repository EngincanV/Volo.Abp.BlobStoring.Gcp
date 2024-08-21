namespace Volo.Abp.BlobStoring.Gcp;

public static class GoogleCloudStorageBlobContainerConfigurationExtensions
{
    public static BlobContainerConfiguration UseGoogleCloudStorage(this BlobContainerConfiguration configuration)
    {
        configuration.ProviderType = typeof(GoogleCloudStorageBlobProvider);

        return configuration;
    }
}