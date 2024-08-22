# Introducing the Google Cloud Storage BLOB Provider

ABP provides a [BLOB Storing System](https://abp.io/docs/latest/framework/infrastructure/blob-storing), which allows you to work with BLOBs. This system is typically used to store file contents in a project and read these file contents when they are needed. Since ABP provides an abstraction to work with BLOBs, it also provides some pre-built storage providers such as [Azure](https://abp.io/docs/latest/framework/infrastructure/blob-storing/azure), [Aws](https://abp.io/docs/latest/framework/infrastructure/blob-storing/aws) and [Aliyun](https://abp.io/docs/latest/framework/infrastructure/blob-storing/aliyun).

Recently, I worked on Google Cloud Storage and wanted to create a new storage provider: [BlobStoring.Providers.GoogleCloudStorage](https://github.com/EngincanV/Volo.Abp.BlobStoring.Gcp)

Google Cloud Storage Provider works seamlessly with ABP's BLOB Storing System and allows you to store BLOBS in Google Cloud Storage.

In this article, I will show you how you can use this provider as the blob storage provider. Let's get started :)

> **Note:** Please be aware that in this article I will only cover how to configure containers to use [Google Cloud Storage](https://cloud.google.com/storage) as the storage provider and I will not mention about the [BLOB Storing System of ABP](https://abp.io/docs/latest/framework/infrastructure/blob-storing).

## Installation & Configuration Steps

You typically need to apply the following four steps to configure Google Cloud Storage as the BLOB storage provider:

1. **First, you need to install the package and configure it:**

* Open a command line (terminal) in the directory of the .csproj file you want to add the `BlobStoring.Providers.GoogleCloudStorage` package and run the following command:

```bash
dotnet add package BlobStoring.Providers.GoogleCloudStorage --version 1.0.0
```

* Then, add the following `DependsOn` statement in the module class of your project:

```csharp
using Volo.Abp.BlobStoring.Gcp;
//...

[DependsOn(typeof(AbpBlobStoringGcpModule))]
public class NewAcmeWebModule : AbpModule
{
        //...
}
```

2. **After installing and configuring the package, you should create a service account key (it's needed to connect with Google Cloud Storage):**

Here are the documents you need to check to create a service account key:

* https://console.cloud.google.com/iam-admin/serviceaccounts
* https://cloud.google.com/iam/docs/keys-create-delete

We need to create a service account key to obtain credentials to connect with [Google Cloud Storage](https://console.cloud.google.com/). When you select to create a **service account key** in the *keys* section, a .JSON file will be downloaded along with all related info (such as **clientEmailAddress**, **privateKey**, and **projectId**).

Alternatively, you can follow the interactive tutorial on the Google Cloud Storage website. Here are the complete steps you need to do (from the interactive tutorial):

![](assets/images/interactive-tutorial.png)

3. **After you follow these instructions, then you can configure the `GoogleCloudStorageBlobOptions` with these values in the `ConfigureServices` method of your module class:**

```csharp
Configure<GoogleCloudStorageBlobOptions>(options =>
{
    options.ProjectId = "<project-id>";
    options.ClientEmail = "<client-email>";
    options.PrivateKey = "<private-key>";
});
```

> **Note**: `PrivateKey` starts with '-----BEGIN PRIVATE KEY-----' and ends with '-----END PRIVATE KEY-----' placeholders.

4. **After specified credentials, you can finally configure the `AbpBlobStoringOptions` in the `ConfigureServices` method of your module class and directly use the [Blob Storage system of ABP](https://abp.io/docs/latest/framework/infrastructure/blob-storing):**

```csharp
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseGoogleCloudStorage();
            });
        });
```

`IBlobContainer` (and `IBlobContainer<TContainer>`) is the main interface to store and read BLOBs. You can refer to ABP's Blob Storing document for more info: https://abp.io/docs/latest/framework/infrastructure/blob-storing

You can see the simple code block below for a simple usage demonstration:

```diff
        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetFileAsync(GetFileInput input)
        {
            var fileDescriptor = await _appFileDescriptorRepository.GetAsync(input.FileId);

+           //get the file as stream
+           var stream = await _blobContainer.GetAsync(fileDescriptor.Id.ToString("N"));

+           //return the stream with the mime-type/content-type
+           return new RemoteStreamContent(stream, fileDescriptor.Name, fileDescriptor.MimeType);
        }

        public virtual async Task<AppFileDescriptorDto> UploadFileAsync(IRemoteStreamContent input)
        {
            var id = GuidGenerator.Create();
            var fileDescriptor = await _appFileDescriptorRepository.InsertAsync(new AppFileDescriptors.AppFileDescriptor(id, input.FileName, input.ContentType));

+           //Upload file with a unique blob name
+           await _blobContainer.SaveAsync(fileDescriptor.Id.ToString("N"), input.GetStream());

            return ObjectMapper.Map<AppFileDescriptors.AppFileDescriptor, AppFileDescriptorDto>(fileDescriptor);
        }
```

## Demo

Here is a gif that shows the BLOB Storing System (with `BlobStoring.Providers.GoogleCloudStorage`) in action:

![](assets/images/demo-1.gif)

### Google Cloud Storage Dashboard

When a new BLOB is trying to be uploaded, the provider first creates the bucket (container) and uploads the file into that bucket.

For the example above, here are the bucket details:

![](assets/images/demo-2.png)

You can list, get, and download all files in this bucket in the Google Cloud dashboard:

![](assets/images/demo-3.png)

## Conclusion

In this article, I showed you how to configure Google Cloud Storage as the blob storage provider. This provider works seamlessly with ABP's BLOB Storing System and if you apply the steps correctly, then you can directly start using it.

Thanks for reading :)