namespace Volo.Abp.BlobStoring.Gcp;

public static class GcpBlobContainerConfigurationExtensions
{
    public static BlobContainerConfiguration UseGcpBlobProvider(this BlobContainerConfiguration configuration)
    {
        configuration.ProviderType = typeof(GcpBlobProvider);

        return configuration;
    }
}