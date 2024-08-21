using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Gcp;

[DependsOn(typeof(AbpBlobStoringModule))]
public class AbpBlobStoringGcpModule : AbpModule
{

}