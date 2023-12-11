namespace BlobStorageDemo2.Services.contracts
{
    public interface IUploadService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
        Task<string> GetBlobSASTOkenByFile(string fileName);

    }
}
