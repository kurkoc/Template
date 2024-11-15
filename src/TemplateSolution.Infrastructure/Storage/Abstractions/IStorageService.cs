using FluentResults;
using TemplateSolution.Infrastructure.Storage.Types;

namespace TemplateSolution.Infrastructure.Storage.Abstractions;

public interface IStorageService
{
    Task<bool> HasBucketExist(string bucketName);
    Task<List<string>> ListBucketsAsync(CancellationToken cancellationToken);
    Task DeleteAsync(string fileKey, string bucketName, CancellationToken cancellationToken);
    Task<byte[]> DownloadAsync(string fileKey, string bucketName, CancellationToken cancellationToken);
    Task<Result<FileUploadResponse>> UploadAsync(string bucketName, byte[] content,  string contentType, CancellationToken cancellationToken);
    Task<Result<FileUploadResponse>> UploadAsync(string bucketName, Stream content, string contentType, CancellationToken cancellationToken);
    Task<Result<FileUploadResponse>> UploadAsync(string bucketName, string content,CancellationToken cancellationToken);
}