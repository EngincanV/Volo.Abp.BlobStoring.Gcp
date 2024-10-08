﻿using Google.Apis.Storage.v1;

namespace Volo.Abp.BlobStoring.Gcp;

/// <summary>
/// Sets credential options to connect with Google Cloud Storage.
/// See: https://cloud.google.com/docs/authentication/provide-credentials-adc
/// </summary>
public class GoogleCloudStorageBlobOptions
{
    /// <summary>
    /// Unique identifier for your project.
    /// For more info see: https://cloud.google.com/resource-manager/docs/creating-managing-projects
    /// </summary>
    public string ProjectId { get; set; } = null!;

    /// <summary>
    /// Email address that generated by the Google Cloud.
    /// </summary>
    public string ClientEmail { get; set; } = null!;

    /// <summary>
    /// Private key that generated by Google Cloud.
    /// Starts with '-----BEGIN PRIVATE KEY-----'
    /// and ends with '-----END PRIVATE KEY-----'
    /// </summary>
    public string PrivateKey { get; set; } = null!;

    /// <summary>
    /// Available OAuth 2.0 scopes.
    /// </summary>
    public List<string> Scopes { get; set; } = [StorageService.Scope.DevstorageFullControl];
}