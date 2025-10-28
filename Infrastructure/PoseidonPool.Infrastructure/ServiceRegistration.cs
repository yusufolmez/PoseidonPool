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
            services.AddSingleton<IAmazonS3>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                var accessKey = configuration?["AWS:AccessKey"] ?? Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
                var secretKey = configuration?["AWS:SecretKey"] ?? Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
                var region = configuration?["AWS:Region"] ?? Environment.GetEnvironmentVariable("AWS_REGION") ?? "us-east-1";

                if (string.IsNullOrWhiteSpace(accessKey) || string.IsNullOrWhiteSpace(secretKey))
                    throw new InvalidOperationException("AWS credentials are not configured. Set AWS:AccessKey and AWS:SecretKey in configuration or environment variables.");

                var creds = new BasicAWSCredentials(accessKey, secretKey);
                var config = new AmazonS3Config { RegionEndpoint = RegionEndpoint.GetBySystemName(region) };
                return new AmazonS3Client(creds, config);
            });
        }
    }
}
