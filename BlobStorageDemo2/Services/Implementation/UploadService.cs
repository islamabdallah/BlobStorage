using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using BlobStorageDemo2.Services.contracts;
using Azure.Storage;
using Microsoft.Extensions.Configuration;

namespace BlobStorageDemo2.Services.Implementation
{
    public class UploadService : IUploadService
    {

        private readonly IConfiguration _configuration;
        public UploadService( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                string _storageConnectionString = _configuration.GetConnectionString("AzureStorage");

                var container = new BlobContainerClient(_storageConnectionString, "benefitfiles");
                var createResponse = await container.CreateIfNotExistsAsync();
                if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                    await container.SetAccessPolicyAsync(PublicAccessType.Blob);
                var blob = container.GetBlobClient(fileName);
                await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
                return blob.Uri.ToString();
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        public async Task<string> GetBlobSASTOkenByFile(string fileName)
        {
            try
            {
                var azureStorageAccount = _configuration.GetSection("AzureStorage:AzureAccount").Value;
                var azureStorageAccessKey = _configuration.GetSection("AzureStorage:AccessKey").Value;
                Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder()
                {
                    BlobContainerName = "BlobContainerName",
                    BlobName = fileName,
                    ExpiresOn = DateTime.UtcNow.AddMinutes(2),//Let SAS token expire after 5 minutes.
                };
                blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read);//User will only be able to read the blob and it's properties
                var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(azureStorageAccount,
                    azureStorageAccessKey)).ToString();
                return sasToken;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
