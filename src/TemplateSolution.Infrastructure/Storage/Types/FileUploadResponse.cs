namespace TemplateSolution.Infrastructure.Storage.Types;

public record FileUploadResponse(string Etag, string BucketName, string FileName);