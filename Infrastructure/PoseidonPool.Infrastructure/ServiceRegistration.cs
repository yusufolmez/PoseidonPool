using Microsoft.Extensions.DependencyInjection;
using PoseidonPool.Application.Abstractions.Token;
using PoseidonPool.Infrastructure.Services.Token;
using Microsoft.Extensions.Configuration;
using PoseidonPool.Application.Abstractions.Storage;
using PoseidonPool.Application.Abstractions.Storage.Amazon;
using PoseidonPool.Infrastructure.Services.Storage;
using PoseidonPool.Infrastructure.Services.Storage.Amazon;
using Amazon.S3;
using Amazon.Runtime;
using Amazon;
using System;

namespace PoseidonPool.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenHandler, TokenHandler>();
            // Storage registrations: IStorage -> AmazonStorage, IStorageService -> StorageService
            services.AddScoped<IStorage, AmazonStorage>();
            services.AddScoped<IStorageService, StorageService>();

            // Register IAmazonS3 client. Credentials/region can come from appsettings or environment variables.
            // Uses credential fallback (environment/instance profile) when explicit keys are not provided.
            services.AddSingleton<IAmazonS3>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                var accessKey = configuration?["AWS:AccessKey"] ?? Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
                var secretKey = configuration?["AWS:SecretKey"] ?? Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
                var region = configuration?["AWS:Region"] ?? Environment.GetEnvironmentVariable("AWS_REGION") ?? "us-east-1";

                // Optional: allow forcing path style (useful when bucket names contain dots or in some endpoints)
                var forcePathStyleStr = configuration?["Storage:Amazon:ForcePathStyle"] ?? Environment.GetEnvironmentVariable("S3_FORCE_PATH_STYLE");
                var forcePathStyle = false;
                if (!string.IsNullOrWhiteSpace(forcePathStyleStr) && bool.TryParse(forcePathStyleStr, out var fps))
                    forcePathStyle = fps;

                AWSCredentials creds;
                if (!string.IsNullOrWhiteSpace(accessKey) && !string.IsNullOrWhiteSpace(secretKey))
                {
                    creds = new BasicAWSCredentials(accessKey, secretKey);
                }
                else
                {
                    // fallback to the SDK credential chain (env, shared credentials file, EC2/ECS roles...)
                    creds = FallbackCredentialsFactory.GetCredentials();
                }

                var s3Config = new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(region),
                    ForcePathStyle = forcePathStyle
                };

                return new AmazonS3Client(creds, s3Config);
            });
        }
    }
}
