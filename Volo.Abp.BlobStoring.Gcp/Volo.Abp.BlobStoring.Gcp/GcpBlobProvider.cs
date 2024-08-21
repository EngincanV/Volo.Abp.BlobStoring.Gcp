using System.Net;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Gcp;

public class GcpBlobProvider : BlobProviderBase, ITransientDependency
{
    private readonly GcpBlobOptions _gcpBlobOptions;
    private readonly ILogger<GcpBlobProvider> _logger;

    public GcpBlobProvider(IOptions<GcpBlobOptions> gcpBlobOptions, ILogger<GcpBlobProvider> logger)
    {
        _logger = logger;
        _gcpBlobOptions = gcpBlobOptions.Value;
    }

    public override async Task SaveAsync(BlobProviderSaveArgs args)
    {
        var storageClient = await GetClientAsync();

        Bucket bucket;

        try
        {
            bucket = await storageClient.GetBucketAsync(args.ContainerName);
        }
        catch (GoogleApiException e) when (e.HttpStatusCode == HttpStatusCode.NotFound)
        {
            bucket = await storageClient.CreateBucketAsync(_gcpBlobOptions.ProjectId, args.ContainerName);
        }
        catch
        {
            throw new UserFriendlyException("Could not connect to the bucket.");
        }

        await storageClient.UploadObjectAsync(bucket.Name, args.BlobName, contentType: "application/octet-stream", args.BlobStream,
            new UploadObjectOptions
            {
                IfGenerationMatch = !args.OverrideExisting ? 0 : 1
            });
    }

    public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        try
        {
            var storageClient = await GetClientAsync();
            
            await storageClient.DeleteObjectAsync(args.ContainerName, args.BlobName);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
        }

        return false;
    }

    public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        var @object = await (await GetClientAsync()).GetObjectAsync(args.ContainerName, args.BlobName);
        
        return @object != null;
    }

    public override async Task<Stream?> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var storageClient = await GetClientAsync();
        
        var @object = await storageClient.GetObjectAsync(args.ContainerName, args.BlobName);
        if (@object == null)
        {
            return null;
        }

        var stream = new MemoryStream();
        
        await storageClient.DownloadObjectAsync(args.ContainerName, args.BlobName, stream);
        
        stream.Seek(0, SeekOrigin.Begin);
        
        return stream;
    }

    private async Task<StorageClient> GetClientAsync()
    {
        var googleCredential = GoogleCredential.FromServiceAccountCredential(
            new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(_gcpBlobOptions.ClientEmail)
                {
                    ProjectId = _gcpBlobOptions.ProjectId,
                    Scopes = new[] { StorageService.Scope.DevstorageFullControl }
                }.FromPrivateKey(_gcpBlobOptions.PrivateKey)
            ));
        
        return await StorageClient.CreateAsync(googleCredential);
    }
}