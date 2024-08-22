# BlobStoring.Providers.GoogleCloudStorage

<a href="https://www.nuget.org/packages/BlobStoring.Providers.GoogleCloudStorage"><img src="https://img.shields.io/nuget/v/BlobStoring.Providers.GoogleCloudStorage?logo=nuget" alt="BlobStoring.Providers.GoogleCloudStorage on Nuget" /></a>

BLOB Storing Google Cloud Storage Provider can store BLOBs in Google Cloud Storage. 

Read the [BLOB Storing document of ABP](https://abp.io/docs/latest/framework/infrastructure/blob-storing) to understand how to use the BLOB storing system. This document only covers how to configure containers to use [Google Cloud Storage](https://cloud.google.com/storage) as the storage provider.

## Steps

You can apply the following steps to configure the `BlobStoring.Providers.GoogleCloudStorage`:

1. **Install the package and configure it:**

* Open a command line (terminal) in the directory of the .csproj file you want to add the `BlobStoring.Providers.GoogleCloudStorage` package and run the following command:

```bash
dotnet add package BlobStoring.Providers.GoogleCloudStorage --version 1.0.0
```

* Then, add the following **DependsOn** statement in the AbpModule class of your project:

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

* https://console.cloud.google.com/iam-admin/serviceaccounts
* https://cloud.google.com/iam/docs/keys-create-delete

You can either follow the documents above or use the interactive tutorial on the Google Cloud website. Here are the complete steps you need to do (from the interactive tutorial):

![image](https://github.com/user-attachments/assets/5f2a870c-e916-40fa-a214-e2a54f017eac)

3. **After you follow these instructions, a JSON file will be downloaded. You should store this .JSON file and configure the `GoogleCloudStorageBlobOptions` with these values:**

```csharp
        Configure<GoogleCloudStorageBlobOptions>(options =>
        {
            options.ProjectId = "<project-id>";
            options.ClientEmail = "<client-email>";
            options.PrivateKey = "<private-key>";
        });
```

> **Note:** `PrivateKey` starts with **'-----BEGIN PRIVATE KEY-----'** and ends with **'-----END PRIVATE KEY-----'** placeholders.

4. **After specified credentials, you can finally configure the `AbpBlobStoringOptions` as follows and directly use the [Blob Storage system of ABP](https://abp.io/docs/latest/framework/infrastructure/blob-storing):**

```csharp
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseGoogleCloudStorage();
            });
        });
```

`IBlobContainer` (and  `IBlobContainer<TContainer>`) is the main interface to store and read BLOBs. Please refer to ABP's Blob Storing document for more info: https://abp.io/docs/latest/framework/infrastructure/blob-storing

## Demo

Here is a gif that shows the Blob Storing System (with BlobStoring.Providers.GoogleCloudStorage) in action:

![see-it-in-action](https://github.com/user-attachments/assets/4213d369-5bed-4a5c-9619-f8d6826deb01)

When a new BLOB is trying to be uploaded, the provider first creates the bucket (container) and uploads the file into that bucket. For the example above, here are the bucket details:

![image](https://github.com/user-attachments/assets/40d36d22-f9d9-4e32-a4eb-e04de7560f42)

You can list, get, and download all files in this bucket in the Google Cloud dashboard:

![image](https://github.com/user-attachments/assets/db8324f7-71ed-4f69-9676-23da51b29d26)


