using FluentResults;

using Minio;
using Minio.DataModel.Args;

using TemplateSolution.Infrastructure.Storage.Abstractions;
using TemplateSolution.Infrastructure.Storage.Types;

namespace TemplateSolution.Infrastructure.Storage.Concretes;

public class MinioStorageService(IMinioClient minioClient) : IStorageService
{
    public async Task DeleteAsync(string fileKey, string bucketName, CancellationToken cancellationToken)
    {
        RemoveObjectArgs removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileKey);
        await minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
    }
    public async Task<byte[]> DownloadAsync(string fileKey, string bucketName, CancellationToken cancellationToken)
    {
        MemoryStream ms = new MemoryStream();
        GetObjectArgs getObjectArgs = new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileKey)
            .WithCallbackStream((stream) =>
            {
                stream.CopyTo(ms);
            });
        ms.Position = 0;
        var objectStat = await minioClient.GetObjectAsync(getObjectArgs, cancellationToken);
        return ms.ToArray();
    }
    public async Task<Result<FileUploadResponse>> UploadAsync(string bucketName, byte[] content, string contentType, CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream(content);
        return await UploadAsync(bucketName, ms, contentType,cancellationToken);
    }
    public async Task<Result<FileUploadResponse>> UploadAsync(string bucketName, Stream content, string contentType, CancellationToken cancellationToken)
    {
        try
        {
            await ListBucketsAsync(cancellationToken);
            await CreateBucketIfNotExists(bucketName);
        
            string fileKey = $"{Guid.NewGuid()}{GetFileExtensionFromContentType(contentType)}";
        
            PutObjectArgs putObjectArgs = new PutObjectArgs()
                .WithContentType(contentType)
                .WithObjectSize(content.Length)
                .WithObject(fileKey)
                .WithBucket(bucketName)
                .WithStreamData(content);
        
            var response = await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            return Result.Ok(new FileUploadResponse(response.Etag, bucketName, fileKey));
        }
        catch (Exception ex)
        {
            return Result.Fail<FileUploadResponse>(ex.Message);
        }
    }
    public async Task<Result<FileUploadResponse>> UploadAsync(string bucketName, string base64, CancellationToken cancellationToken)
    {
        var (contentType, content) = ParseBase64Data(base64);
        byte[] fileBytes = Convert.FromBase64String(content);
        return await UploadAsync(bucketName, fileBytes, contentType,  cancellationToken);
    }
    public async Task<bool> HasBucketExist(string bucketName)
    {
        return await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
    }
    public async Task<List<string>> ListBucketsAsync(CancellationToken cancellationToken)
    {
        var bucketsResult = await minioClient.ListBucketsAsync(cancellationToken);
        return bucketsResult.Buckets.Select(bucket => bucket.Name).ToList();
    }
    private async Task CreateBucketIfNotExists(string bucketName)
    {
        bool hasExist = await HasBucketExist(bucketName);
        if (hasExist) return;

        await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
    }
    private (string ContentType, string Data) ParseBase64Data(string base64Data)
    {
        const string DataPrefix = "data:";
        const string Base64Prefix = ";base64,";

        if (string.IsNullOrWhiteSpace(base64Data) || 
            !base64Data.StartsWith(DataPrefix) || 
            !base64Data.Contains(Base64Prefix))
        {
            throw new ArgumentException("Invalid base64 format.");
        }

        ReadOnlySpan<char> base64Span = base64Data.AsSpan();
        int contentTypeStartIndex = DataPrefix.Length;
        int contentTypeEndIndex = base64Span.IndexOf(Base64Prefix);
        
        var contentType = base64Span.Slice(contentTypeStartIndex, contentTypeEndIndex - contentTypeStartIndex).ToString();
        var data = base64Span.Slice(contentTypeEndIndex + Base64Prefix.Length).ToString();

        return (contentType, data);
    }
    private string GetFileExtensionFromContentType(string contentType) =>
        contentType.ToLower() switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/gif" => ".gif",
            "image/bmp" => ".bmp",
            "image/tiff" => ".tiff",
            "image/webp" => ".webp",
            "image/svg+xml" => ".svg",
            "application/pdf" => ".pdf",
            "application/msword" => ".doc",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => ".docx",
            "application/vnd.ms-excel" => ".xls",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => ".xlsx",
            "application/vnd.ms-powerpoint" => ".ppt",
            "application/vnd.openxmlformats-officedocument.presentationml.presentation" => ".pptx",
            "text/plain" => ".txt",
            "text/html" => ".html",
            _ => throw new ArgumentException("Unsupported content type")
        };
}