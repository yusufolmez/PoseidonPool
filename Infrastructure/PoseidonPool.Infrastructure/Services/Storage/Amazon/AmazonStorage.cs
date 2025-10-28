using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoseidonPool.Application.Abstractions.Storage;
using PoseidonPool.Application.Abstractions.Storage.Amazon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PoseidonPool.Infrastructure.Services.Storage.Amazon
{
    public class AmazonStorage : Storage, IAmazonStorage
    {
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<AmazonStorage> _logger;
        private readonly string _defaultBucket;

        public AmazonStorage(IAmazonS3 s3Client, IConfiguration configuration, ILogger<AmazonStorage> logger)
        {
            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
            _logger = logger;
            _defaultBucket = configuration?["Storage:Amazon:BucketName"] ?? string.Empty;
        }

        private string ResolveBucket(string pathOrContainerName)
        {
            return string.IsNullOrWhiteSpace(pathOrContainerName) ? _defaultBucket : pathOrContainerName;
        }

        // asynchronous helper for Storage.FileRenameAsync
        private async Task<bool> HasFileAsync(string pathOrContainerName, string fileName)
        {
            var bucket = ResolveBucket(pathOrContainerName);
            if (string.IsNullOrWhiteSpace(bucket))
                throw new InvalidOperationException("Bucket name is not configured.");

            try
            {
                var response = await _s3Client.GetObjectMetadataAsync(bucket, fileName);
                return response.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return false;
                _logger?.LogError(ex, "S3 HasFile kontrolü sırasında hata");
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "S3 HasFile bilinmeyen hata");
                throw;
            }
        }

        bool IStorage.HasFile(string pathOrContainerName, string fileName)
            => HasFileAsync(pathOrContainerName, fileName).GetAwaiter().GetResult();

        public async Task DeleteAsync(string pathOrContainerName, string fileName)
        {
            var bucket = ResolveBucket(pathOrContainerName);
            if (string.IsNullOrWhiteSpace(bucket))
                throw new InvalidOperationException("Bucket name is not configured.");

            var key = BuildKey(pathOrContainerName, fileName);
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = bucket,
                    Key = key
                };
                await _s3Client.DeleteObjectAsync(request);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "S3 DeleteAsync hatası");
                throw;
            }
        }

        public List<string> GetFiles(string pathOrContainerName)
        {
            var bucket = ResolveBucket(pathOrContainerName);
            if (string.IsNullOrWhiteSpace(bucket))
                throw new InvalidOperationException("Bucket name is not configured.");

            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = bucket,
                    Prefix = ""
                };

                var result = _s3Client.ListObjectsV2Async(request).GetAwaiter().GetResult();
                return result.S3Objects.Select(o => o.Key).ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "S3 GetFiles hatası");
                throw;
            }
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            var bucket = ResolveBucket(pathOrContainerName);
            if (string.IsNullOrWhiteSpace(bucket))
                throw new InvalidOperationException("Bucket name is not configured.");

            var uploaded = new List<(string fileName, string pathOrContainerName)>();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0) continue;

                var safeFileName = await FileRenameAsync(pathOrContainerName, file.FileName, HasFileAsync);
                var key = BuildKey(pathOrContainerName, safeFileName);

                try
                {
                    using var stream = file.OpenReadStream();
                    var putRequest = new PutObjectRequest
                    {
                        BucketName = bucket,
                        Key = key,
                        InputStream = stream,
                        ContentType = file.ContentType,
                        AutoCloseStream = true
                    };

                    var response = await _s3Client.PutObjectAsync(putRequest);
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        uploaded.Add((safeFileName, pathOrContainerName));
                    }
                    else
                    {
                        _logger?.LogWarning("S3 yükleme beklenmeyen status: {Status} key:{Key}", response.HttpStatusCode, key);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "S3 Upload hatası for file {File}", file.FileName);
                    throw;
                }
            }

            return uploaded;
        }

        private string BuildKey(string pathOrContainerName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(pathOrContainerName))
                return fileName;

            var prefix = pathOrContainerName.Trim('/');
            return $"{prefix}/{fileName}";
        }
    }
}
