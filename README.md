# Volo.Abp.BlobStoring.Gcp

BLOB Storing Google Cloud Storage Provider can store BLOBs in Google Cloud Storage. 

Read the [BLOB Storing document of ABP](https://abp.io/docs/latest/framework/infrastructure/blob-storing) to understand how to use the BLOB storing system. This document only covers how to configure containers to use the Google Cloud Storage as the storage provider.

## Steps

1. Create a service account key:
* https://console.cloud.google.com/iam-admin/serviceaccounts
* https://cloud.google.com/iam/docs/keys-create-delete

> Also, you can use the interactive tutorial on the Google Cloud website. Here is a complete steps you need to do:

![image](https://github.com/user-attachments/assets/304ac13e-d19f-4220-a777-6f35b5db05b7)

2. After you follow these instructions, a JSON file will be downloaded. You should store this .JSON file and configure the `GoogleCloudStorageBlobOptions` with these values:

```csharp
        Configure<GcpBlobOptions>(options =>
        {
            options.ProjectId = "<project-id>";
            options.ClientEmail = "<client-email>";
            options.PrivateKey = "<private-key>";
        });
```

3. After specified credentials, you can finally configure the `AbpBlobStoringOptions` as follows and directly use the [Blob Storage system of ABP](https://abp.io/docs/latest/framework/infrastructure/blob-storing):

```csharp
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseGcpBlobProvider();
            });
        });
```
