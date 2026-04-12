using Azure.Storage.Blobs;
using InvoiceProcessor.Application.Interfaces;

namespace InvoiceProcessor.Infrastructure.Storage
{

    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _container;

        public BlobStorageService(string connectionString, string containerName)
        {
            var client = new BlobServiceClient(connectionString);
            _container = client.GetBlobContainerClient(containerName);
            _container.CreateIfNotExists();
        }

        public async Task<string> UploadAsync(Stream file, string fileName, CancellationToken cancellationToken)
        {
            var blobName = $"{Guid.NewGuid()}-{fileName}";
            var blobClient = _container.GetBlobClient(blobName);

            await blobClient.UploadAsync(file, overwrite: true, cancellationToken: cancellationToken);

            return blobName;
        }
    }
}
