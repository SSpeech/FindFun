using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FindFun.Server.Validations;

namespace FindFun.Server.Shared;

public class FileUpLoad(
    BlobServiceClient blobServiceClient,
    IConfiguration configuration)
{
    public async Task<Result<string>> FileUpLoader(IFormFile files, string folderName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var containerName = configuration["BlobStorage:ContainerName"];
        if (string.IsNullOrWhiteSpace(containerName))
            throw new InvalidOperationException("Configuration key 'BlobStorage:ContainerName' not found.");

        var container = blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(publicAccessType: PublicAccessType.Blob, cancellationToken: cancellationToken);

        var fileExtension = Path.GetExtension(files.FileName);
        var safeFileName = $"{Guid.NewGuid()}{fileExtension}".ToLowerInvariant();

        var blobName = $"{folderName.Trim('/')}/{safeFileName}";
        var blobClient = container.GetBlobClient(blobName);

        await using var stream = files.OpenReadStream();
        await blobClient.UploadAsync( stream, new BlobHttpHeaders { ContentType = files.ContentType },cancellationToken: cancellationToken);

        return Result<string>.Success(blobClient.Uri.ToString());
    }

    public async Task<Result<bool>> DeleteFileAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        var containerName = configuration["BlobStorage:ContainerName"];
        if (string.IsNullOrWhiteSpace(containerName))
            throw new InvalidOperationException("Configuration key 'BlobStorage:ContainerName' not found.");

        var container = blobServiceClient.GetBlobContainerClient(containerName);

        var blobName = relativePath.TrimStart('/');
        var blobClient = container.GetBlobClient(blobName);

        var response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        return response.Value ? Result<bool>.Success(true) : Result<bool>.Failure(false);
    }

    public async Task<List<Result<string>>> FilesUpLoader(IFormFileCollection files, string folderName, CancellationToken cancellationToken)
    {
        var result = files.Select(file => FileUpLoader(file, folderName, cancellationToken));
        return [.. await Task.WhenAll(result)];
    }
}
