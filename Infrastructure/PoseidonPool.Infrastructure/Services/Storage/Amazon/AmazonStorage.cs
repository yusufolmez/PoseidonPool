using Amazon.S3;
using Amazon.S3.Util;
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
            _logger?.LogDebug("AmazonStorage initialized. DefaultBucket={Bucket}", _defaultBucket);
        }

        private string ResolveBucket(string pathOrContainerName)
        {
            if (string.IsNullOrWhiteSpace(_defaultBucket))
                throw new InvalidOperationException("Default bucket is not configured. Set Storage:Amazon:BucketName in configuration.");

            return _defaultBucket;
        }

        private async Task<bool> HasFileAsync(string pathOrContainerName, string fileName)
        {
            var bucket = ResolveBucket(pathOrContainerName);
            if (string.IsNullOrWhiteSpace(bucket))
                throw new InvalidOperationException("Bucket name is not configured.");

            try
            {
                var key = BuildKey(pathOrContainerName, fileName);
                var response = await _s3Client.GetObjectMetadataAsync(bucket, key);
                return response.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return false;

                if (ex.StatusCode == HttpStatusCode.MovedPermanently)
                {
                    _logger?.LogError(ex, "S3 returned MovedPermanently when checking object. Bucket:'{Bucket}' ClientRegion:'{ClientRegion}'", bucket, _s3Client.Config.RegionEndpoint?.SystemName);
                    throw new InvalidOperationException($"S3 returned MovedPermanently for bucket '{bucket}'. Check that AWS:Region matches the bucket's region and consider enabling Storage:Amazon:ForcePathStyle. Original: {ex.Message}", ex);
                }

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
                var prefix = string.IsNullOrWhiteSpace(pathOrContainerName) ? string.Empty : pathOrContainerName.Trim('/');

                var request = new ListObjectsV2Request
                {
                    BucketName = bucket,
                    Prefix = prefix
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

            await EnsureBucketExistsAsync(bucket).ConfigureAwait(false);

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

        private async Task EnsureBucketExistsAsync(string bucket)
        {
            if (string.IsNullOrWhiteSpace(bucket))
                throw new InvalidOperationException("Bucket name is not configured.");

            try
            {
                var exists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucket).ConfigureAwait(false);
                if (exists)
                    return;

                try
                {
                    var loc = await _s3Client.GetBucketLocationAsync(new GetBucketLocationRequest { BucketName = bucket }).ConfigureAwait(false);
                    _logger?.LogError("GetBucketLocation response for bucket '{Bucket}': {Location}", bucket, loc.Location);
                }
                catch (AmazonS3Exception ex)
                {
                    _logger?.LogError(ex, "GetBucketLocation failed for bucket '{Bucket}'", bucket);
                }

                throw new InvalidOperationException($"S3 bucket '{bucket}' does not exist or is not accessible with provided AWS credentials. Verify AWS keys, region and IAM permissions.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Bucket existence check failed for '{Bucket}'", bucket);
                throw;
            }
        }
    }
}
